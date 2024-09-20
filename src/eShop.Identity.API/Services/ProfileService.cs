namespace eShop.Identity.API.Services;

public class ProfileService(UserManager<ApplicationUser> userManager) : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

        var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault()?.Value;

        var user = await this._userManager.FindByIdAsync(subjectId!) ?? throw new ArgumentException("Invalid subject identifier");
        var claims = this.GetClaimsFromUser(user);
        context.IssuedClaims = [.. claims];
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

        var subjectId = subject.Claims.Where(x => x.Type == "sub").FirstOrDefault()?.Value;
        var user = await this._userManager.FindByIdAsync(subjectId!);

        context.IsActive = false;

        if (user != null)
        {
            if (this._userManager.SupportsUserSecurityStamp)
            {
                var security_stamp = subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                if (security_stamp != null)
                {
                    var db_security_stamp = await this._userManager.GetSecurityStampAsync(user);
                    if (db_security_stamp != security_stamp)
                        return;
                }
            }

            context.IsActive =
                !user.LockoutEnabled ||
                !user.LockoutEnd.HasValue ||
                user.LockoutEnd <= DateTime.UtcNow;
        }
    }

    private List<Claim> GetClaimsFromUser(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtClaimTypes.Subject, user.Id),
            new(JwtClaimTypes.PreferredUserName, user.UserName!),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new(JwtClaimTypes.GivenName, user.FirstName),
            new(JwtClaimTypes.FamilyName, user.LastName)
        };

        if (this._userManager.SupportsUserEmail)
        {
            claims.AddRange(
            [
                new Claim(JwtClaimTypes.Email, user.Email!),
                new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
            ]);
        }

        if (this._userManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
        {
            claims.AddRange(
            [
                new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber),
                new Claim(JwtClaimTypes.PhoneNumberVerified, user.PhoneNumberConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
            ]);
        }

        return claims;
    }
}
