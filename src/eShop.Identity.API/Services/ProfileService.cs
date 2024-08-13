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
            new(JwtRegisteredClaimNames.UniqueName, user.UserName!)
        };

        if (!string.IsNullOrWhiteSpace(user.Name))
            claims.Add(new Claim("name", user.Name));

        if (!string.IsNullOrWhiteSpace(user.LastName))
            claims.Add(new Claim("last_name", user.LastName));

        if (!string.IsNullOrWhiteSpace(user.CardNumber))
            claims.Add(new Claim("card_number", user.CardNumber));

        if (!string.IsNullOrWhiteSpace(user.CardHolderName))
            claims.Add(new Claim("card_holder", user.CardHolderName));

        if (!string.IsNullOrWhiteSpace(user.SecurityNumber))
            claims.Add(new Claim("card_security_number", user.SecurityNumber));

        if (!string.IsNullOrWhiteSpace(user.Expiration))
            claims.Add(new Claim("card_expiration", user.Expiration));

        if (!string.IsNullOrWhiteSpace(user.City))
            claims.Add(new Claim("address_city", user.City));

        if (!string.IsNullOrWhiteSpace(user.Country))
            claims.Add(new Claim("address_country", user.Country));

        if (!string.IsNullOrWhiteSpace(user.State))
            claims.Add(new Claim("address_state", user.State));

        if (!string.IsNullOrWhiteSpace(user.Street))
            claims.Add(new Claim("address_street", user.Street));

        if (!string.IsNullOrWhiteSpace(user.ZipCode))
            claims.Add(new Claim("address_zip_code", user.ZipCode));

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
