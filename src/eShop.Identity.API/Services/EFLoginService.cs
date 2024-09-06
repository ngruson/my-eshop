namespace eShop.Identity.API.Services;

public class EFLoginService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : ILoginService<ApplicationUser>
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

    public async Task<ApplicationUser?> FindByUsername(string user)
    {
        return await this._userManager.FindByEmailAsync(user);
    }

    public async Task<bool> ValidateCredentials(ApplicationUser user, string password)
    {
        return await this._userManager.CheckPasswordAsync(user, password);
    }

    public Task SignIn(ApplicationUser user)
    {
        return this._signInManager.SignInAsync(user, true);
    }

    public Task SignInAsync(ApplicationUser user, AuthenticationProperties properties, string authenticationMethod)
    {
        return this._signInManager.SignInAsync(user, properties, authenticationMethod);
    }
}
