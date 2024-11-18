// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace eShop.Identity.API.Quickstart.Grants;

/// <summary>
/// This sample controller allows a user to revoke grants given to clients
/// </summary>
[SecurityHeaders]
[Authorize]
public class GrantsController(IIdentityServerInteractionService interaction,
    IClientStore clients,
    IResourceStore resources,
    IEventService events) : Controller
{
    /// <summary>
    /// Show list of grants
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return this.View("Index", await this.BuildViewModelAsync());
    }

    /// <summary>
    /// Handle postback to revoke a client
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Revoke(string clientId)
    {
        await interaction.RevokeUserConsentAsync(clientId);
        await events.RaiseAsync(new GrantsRevokedEvent(this.User.GetSubjectId(), clientId));

        return this.RedirectToAction("Index");
    }

    private async Task<GrantsViewModel> BuildViewModelAsync()
    {
        IEnumerable<Grant> grants = await interaction.GetAllUserGrantsAsync();

        List<GrantViewModel> list = [];
        foreach (Grant grant in grants)
        {
            Client? client = await clients.FindClientByIdAsync(grant.ClientId);
            if (client != null)
            {
                Resources resourcesByScope = await resources.FindResourcesByScopeAsync(grant.Scopes);

                GrantViewModel item = new()
                {
                    ClientId = client.ClientId,
                    ClientName = client.ClientName ?? client.ClientId,
                    ClientLogoUrl = client.LogoUri,
                    ClientUrl = client.ClientUri,
                    Description = grant.Description,
                    Created = grant.CreationTime,
                    Expires = grant.Expiration,
                    IdentityGrantNames = resourcesByScope.IdentityResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
                    ApiGrantNames = resourcesByScope.ApiScopes.Select(x => x.DisplayName ?? x.Name).ToArray()
                };

                list.Add(item);
            }
        }

        return new GrantsViewModel
        {
            Grants = list
        };
    }
}
