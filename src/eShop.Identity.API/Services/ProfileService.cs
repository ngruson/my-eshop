namespace eShop.Identity.API.Services;

public class ProfileService(UserManager<ApplicationUser> userManager) : IProfileService
{
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        ArgumentNullException.ThrowIfNull(context.Subject, nameof(context.Subject));        

        string? subjectId = context.Subject.Claims.Where(x => x.Type == "sub").FirstOrDefault()?.Value;

        ApplicationUser user = await userManager.FindByIdAsync(subjectId!) ?? throw new ArgumentException("Invalid subject identifier");
        List<Claim> claims = this.GetClaimsFromUser(user);
        context.IssuedClaims = [.. claims];
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        ArgumentNullException.ThrowIfNull(context.Subject, nameof(context.Subject));

        string? subjectId = context.Subject.Claims.Where(x => x.Type == "sub").FirstOrDefault()?.Value;
        ApplicationUser? user = await userManager.FindByIdAsync(subjectId!);

        context.IsActive = false;

        if (user != null)
        {
            if (userManager.SupportsUserSecurityStamp)
            {
                string? security_stamp = context.Subject.Claims.Where(c => c.Type == "security_stamp").Select(c => c.Value).SingleOrDefault();
                if (security_stamp != null)
                {
                    var db_security_stamp = await userManager.GetSecurityStampAsync(user);
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
            new(JwtClaimTypes.GivenName, user.FirstName!),
            new(JwtClaimTypes.FamilyName, user.LastName!)
        };

        if (userManager.SupportsUserEmail)
        {
            claims.AddRange(
            [
                new Claim(JwtClaimTypes.Email, user.Email!),
                new Claim(JwtClaimTypes.EmailVerified, user.EmailConfirmed ? "true" : "false", ClaimValueTypes.Boolean)
            ]);
        }

        if (userManager.SupportsUserPhoneNumber && !string.IsNullOrWhiteSpace(user.PhoneNumber))
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
