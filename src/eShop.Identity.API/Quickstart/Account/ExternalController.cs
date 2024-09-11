namespace IdentityServerHost.Quickstart.UI;

[SecurityHeaders]
[AllowAnonymous]
public class ExternalController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IIdentityServerInteractionService interaction,
    IClientStore clientStore,
    IEventService events,
    ILogger<ExternalController> logger) : Controller
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IIdentityServerInteractionService _interaction = interaction;
    private readonly IClientStore _clientStore = clientStore;
    private readonly IEventService _events = events;
    private readonly ILogger<ExternalController> _logger = logger;

    /// <summary>
    /// initiate roundtrip to external authentication provider
    /// </summary>
    [HttpGet]
    public IActionResult Challenge(string scheme, string returnUrl)
    {
        if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

        // validate returnUrl - either it is a valid OIDC URL or back to a local page
        if (this.Url.IsLocalUrl(returnUrl) == false && this._interaction.IsValidReturnUrl(returnUrl) == false)
        {
            // user might have clicked on a malicious link - should be logged
            throw new Exception("invalid return URL");
        }

        // start challenge and roundtrip the return URL and scheme 
        var props = new AuthenticationProperties
        {
            RedirectUri = this.Url.Action(nameof(Callback)),
            Items =
                {
                    { "returnUrl", returnUrl },
                    { "scheme", scheme },
                }
        };

        return this.Challenge(props, scheme);

    }

    /// <summary>
    /// Post processing of external authentication
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Callback()
    {
        // read external identity from the temporary cookie
        var result = await this.HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);
        if (result?.Succeeded != true)
        {
            throw new Exception("External authentication error");
        }

        if (this._logger.IsEnabled(LogLevel.Debug))
        {
            var externalClaims = result.Principal!.Claims.Select(c => $"{c.Type}: {c.Value}");
            this._logger.LogDebug("External claims: {@claims}", externalClaims);
        }

        // lookup our user and external provider info
        var (user, provider, providerUserId, claims) = await this.FindUserFromExternalProviderAsync(result);
        // this might be where you might initiate a custom workflow for user registration
        // in this sample we don't show how that would be done, as our sample implementation
        // simply auto-provisions new external user
        user ??= await this.AutoProvisionUserAsync(provider, providerUserId, claims);

        // this allows us to collect any additional claims or properties
        // for the specific protocols used and store them in the local auth cookie.
        // this is typically used to store data needed for signout from those protocols.
        var additionalLocalClaims = new List<Claim>();
        var localSignInProps = new AuthenticationProperties();
        ProcessLoginCallback(result, additionalLocalClaims, localSignInProps);

        // issue authentication cookie for user
        // we must issue the cookie manually, and can't use the SignInManager because
        // it doesn't expose an API to issue additional claims from the login workflow
        var principal = await this._signInManager.CreateUserPrincipalAsync(user);
        additionalLocalClaims.AddRange(principal.Claims);
        var name = principal.FindFirst(JwtClaimTypes.Name)?.Value ?? user.Id;

        var identityServerUser = new IdentityServerUser(user.Id)
        {
            DisplayName = name,
            IdentityProvider = provider,
            AdditionalClaims = additionalLocalClaims
        };

        await this.HttpContext.SignInAsync(identityServerUser, localSignInProps);

        // delete temporary cookie used during external authentication
        await this.HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        // retrieve return URL
        var returnUrl = result.Properties!.Items["returnUrl"] ?? "~/";

        // check if external login is in the context of an OIDC request
        var context = await this._interaction.GetAuthorizationContextAsync(returnUrl);
        await this._events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id, name, true, context?.Client.ClientId));

        if (context != null)
        {
            if (context.IsNativeClient())
            {
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                return this.LoadingPage("Redirect", returnUrl);
            }
        }

        return this.Redirect(returnUrl);
    }

    private async Task<(ApplicationUser user, string provider, string providerUserId, IEnumerable<Claim> claims)>
        FindUserFromExternalProviderAsync(AuthenticateResult result)
    {
        ClaimsPrincipal externalUser = result.Principal!;

        // try to determine the unique id of the external user (issued by the provider)
        // the most common claim type for that are the sub claim and the NameIdentifier
        // depending on the external provider, some other claim type might be used
        Claim? userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                          externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                          throw new Exception("Unknown user id");

        // remove the user id claim so we don't include it as an extra claim if/when we provision the user
        var claims = externalUser.Claims.ToList();
        claims.Remove(userIdClaim);

        string provider = result.Properties!.Items["scheme"]!;
        string providerUserId = userIdClaim.Value;

        // find external user
        ApplicationUser? user = await this._userManager.FindByLoginAsync(provider, providerUserId);

        return (user!, provider!, providerUserId!, claims);
    }

    private async Task<ApplicationUser> AutoProvisionUserAsync(string provider, string providerUserId, IEnumerable<Claim> claims)
    {
        // create a list of claims that we want to transfer into our store
        var filtered = new List<Claim>();

        // user's display name
        string? name = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value ??
            claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        string? first = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value ??
                claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
        string? last = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value ??
            claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;

        if (name != null)
        {
            filtered.Add(new Claim(JwtClaimTypes.Name, name));

            first ??= name.Split(' ')[0];
            last ??= name.Split(' ')[1];
        }
        else
        {
            if (first != null && last != null)
            {
                name = first + " " + last;
                filtered.Add(new Claim(JwtClaimTypes.Name, name));
            }
            else if (first != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, first));
            }
            else if (last != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, last));
            }
        }

        // email
        string? email = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ??
           claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ??
           claims.FirstOrDefault(x => x.Type == JwtClaimTypes.PreferredUserName)?.Value;
        if (email != null)
        {
            filtered.Add(new Claim(JwtClaimTypes.Email, email));
        }

        var user = new ApplicationUser
        {
            UserName = Guid.NewGuid().ToString(),
            Name = name,
            LastName = last,
            Email = email,
            EmailConfirmed = true
        };
        var identityResult = await this._userManager.CreateAsync(user);
        if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

        if (filtered.Count > 0)
        {
            identityResult = await this._userManager.AddClaimsAsync(user, filtered);
            if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
        }

        identityResult = await this._userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));
        if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

        return user;
    }

    // if the external login is OIDC-based, there are certain things we need to preserve to make logout work
    // this will be different for WS-Fed, SAML2p or other protocols
    private static void ProcessLoginCallback(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
    {
        // if the external system sent a session id claim, copy it over
        // so we can use it for single sign-out
        var sid = externalResult.Principal!.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
        if (sid != null)
        {
            localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
        }

        // if the external provider issued an id_token, we'll keep it for signout
        string? idToken = externalResult.Properties!.GetTokenValue("id_token");
        if (idToken != null)
        {
            localSignInProps.StoreTokens([new AuthenticationToken { Name = "id_token", Value = idToken }]);
        }
    }
}
