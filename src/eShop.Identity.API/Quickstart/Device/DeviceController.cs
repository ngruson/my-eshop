// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using eShop.Identity.API.Quickstart.Consent;

namespace eShop.Identity.API.Quickstart.Device;

[Authorize]
[SecurityHeaders]
public class DeviceController(
    IDeviceFlowInteractionService interaction,
    IEventService eventService,
    IOptions<IdentityServerOptions> options) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string userCodeParamName = options.Value.UserInteraction.DeviceVerificationUserCodeParameter;
        string? userCode = this.Request.Query[userCodeParamName];
        if (string.IsNullOrWhiteSpace(userCode))
        {
            return this.View("UserCodeCapture");
        }

        DeviceAuthorizationViewModel? vm = await this.BuildViewModelAsync(userCode);
        if (vm == null)
        {
            return this.View("Error");
        }

        vm.ConfirmUserCode = true;
        return this.View("UserCodeConfirmation", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UserCodeCapture(string userCode)
    {
        DeviceAuthorizationViewModel? vm = await this.BuildViewModelAsync(userCode);
        if (vm == null)
        {
            return this.View("Error");
        }

        return this.View("UserCodeConfirmation", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Callback(DeviceAuthorizationInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        ProcessConsentResult result = await this.ProcessConsent(model);
        if (result.HasValidationError)
        {
            return this.View("Error");
        }

        return this.View("Success");
    }

    private async Task<ProcessConsentResult> ProcessConsent(DeviceAuthorizationInputModel model)
    {
        ProcessConsentResult result = new();

        DeviceFlowAuthorizationRequest? request = await interaction.GetAuthorizationContextAsync(model.UserCode);
        if (request == null)
        {
            return result;
        }

        ConsentResponse? grantedConsent = null;

        // user clicked 'no' - send back the standard 'access_denied' response
        if (model.Button == "no")
        {
            grantedConsent = new ConsentResponse { Error = AuthorizationError.AccessDenied };

            // emit event
            await eventService.RaiseAsync(new ConsentDeniedEvent(
                this.User.GetSubjectId(),
                request.Client.ClientId,
                request.ValidatedResources.RawScopeValues));
        }
        // user clicked 'yes' - validate the data
        else if (model.Button == "yes")
        {
            // if the user consented to some scope, build the response model
            if (model.ScopesConsented != null && model.ScopesConsented.Any())
            {
                IEnumerable<string> scopes = model.ScopesConsented;
                if (ConsentOptions.EnableOfflineAccess == false)
                {
                    scopes = scopes.Where(x => x != IdentityServerConstants.StandardScopes.OfflineAccess);
                }

                grantedConsent = new ConsentResponse
                {
                    RememberConsent = model.RememberConsent,
                    ScopesValuesConsented = scopes.ToArray(),
                    Description = model.Description
                };

                // emit event
                await eventService.RaiseAsync(new ConsentGrantedEvent(this.User.GetSubjectId(), request.Client.ClientId, request.ValidatedResources.RawScopeValues, grantedConsent.ScopesValuesConsented, grantedConsent.RememberConsent));
            }
            else
            {
                result.ValidationError = ConsentOptions.MustChooseOneErrorMessage;
            }
        }
        else
        {
            result.ValidationError = ConsentOptions.InvalidSelectionErrorMessage;
        }

        if (grantedConsent != null)
        {
            // communicate outcome of consent back to identityserver
            await interaction.HandleRequestAsync(model.UserCode, grantedConsent);

            // indicate that's it ok to redirect back to authorization endpoint
            result.RedirectUri = model.ReturnUrl;
            result.Client = request.Client;
        }
        else
        {
            // we need to redisplay the consent UI
            result.ViewModel = await this.BuildViewModelAsync(model.UserCode, model);
        }

        return result;
    }

    private async Task<DeviceAuthorizationViewModel?> BuildViewModelAsync(string userCode, DeviceAuthorizationInputModel? model = null)
    {
        DeviceFlowAuthorizationRequest? request = await interaction.GetAuthorizationContextAsync(userCode);
        if (request != null)
        {
            return this.CreateConsentViewModel(userCode, model, request);
        }

        return null;
    }

    private DeviceAuthorizationViewModel CreateConsentViewModel(string userCode, DeviceAuthorizationInputModel? model, DeviceFlowAuthorizationRequest request)
    {
        DeviceAuthorizationViewModel vm = new()
        {
            UserCode = userCode,
            Description = model?.Description,

            RememberConsent = model?.RememberConsent ?? true,
            ScopesConsented = model?.ScopesConsented ?? [],

            ClientName = request.Client.ClientName ?? request.Client.ClientId,
            ClientUrl = request.Client.ClientUri,
            ClientLogoUrl = request.Client.LogoUri,
            AllowRememberConsent = request.Client.AllowRememberConsent
        };

        vm.IdentityScopes = request.ValidatedResources.Resources.IdentityResources.Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null)).ToArray();

        List<ScopeViewModel> apiScopes = [];
        foreach (ParsedScopeValue parsedScope in request.ValidatedResources.ParsedScopes)
        {
            ApiScope apiScope = request.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);
            if (apiScope != null)
            {
                ScopeViewModel scopeVm = this.CreateScopeViewModel(parsedScope, apiScope, vm.ScopesConsented.Contains(parsedScope.RawValue) || model == null);
                apiScopes.Add(scopeVm);
            }
        }
        if (ConsentOptions.EnableOfflineAccess && request.ValidatedResources.Resources.OfflineAccess)
        {
            apiScopes.Add(GetOfflineAccessScope(vm.ScopesConsented.Contains(IdentityServerConstants.StandardScopes.OfflineAccess) || model == null));
        }
        vm.ApiScopes = apiScopes;

        return vm;
    }

    private static ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
    {
        return new ScopeViewModel
        {
            Value = identity.Name,
            DisplayName = identity.DisplayName ?? identity.Name,
            Description = identity.Description,
            Emphasize = identity.Emphasize,
            Required = identity.Required,
            Checked = check || identity.Required
        };
    }

    public ScopeViewModel CreateScopeViewModel(ParsedScopeValue parsedScopeValue, ApiScope apiScope, bool check)
    {
        return new ScopeViewModel
        {
            Value = parsedScopeValue.RawValue,
            DisplayName = apiScope.DisplayName ?? apiScope.Name,
            Description = apiScope.Description,
            Emphasize = apiScope.Emphasize,
            Required = apiScope.Required,
            Checked = check || apiScope.Required
        };
    }
    private static ScopeViewModel GetOfflineAccessScope(bool check)
    {
        return new ScopeViewModel
        {
            Value = IdentityServerConstants.StandardScopes.OfflineAccess,
            DisplayName = ConsentOptions.OfflineAccessDisplayName,
            Description = ConsentOptions.OfflineAccessDescription,
            Emphasize = true,
            Checked = check
        };
    }
}
