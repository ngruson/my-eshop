// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace eShop.Identity.API.Quickstart.Diagnostics;

[SecurityHeaders]
[Authorize]
public class DiagnosticsController : Controller
{
    public async Task<IActionResult> Index()
    {
        string? localIpAddress = this.HttpContext.Connection?.LocalIpAddress?.ToString();
        string[] localAddresses = ["127.0.0.1", "::1", localIpAddress ?? string.Empty];
        if (!localAddresses.Contains(this.HttpContext.Connection?.RemoteIpAddress?.ToString()))
        {
            return this.NotFound();
        }

        DiagnosticsViewModel model = new(await this.HttpContext.AuthenticateAsync());
        return this.View(model);
    }
}
