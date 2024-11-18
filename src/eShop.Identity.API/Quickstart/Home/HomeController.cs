// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace eShop.Identity.API.Quickstart.Home
{
    [SecurityHeaders]
    [AllowAnonymous]
    public class HomeController(
        IIdentityServerInteractionService interaction,
        IWebHostEnvironment environment,
        ILogger<HomeController> logger) : Controller
    {
        public IActionResult Index()
        {
            if (environment.IsDevelopment())
            {
                // only show in development
                return this.View();
            }

            logger.LogInformation("Homepage is disabled in production. Returning 404.");
            return this.NotFound();
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            ErrorViewModel vm = new();

            // retrieve error details from identityserver
            ErrorMessage? message = await interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;

                if (!environment.IsDevelopment())
                {
                    // only show in development
                    message.ErrorDescription = null;
                }
            }

            return this.View("Error", vm);
        }
    }
}
