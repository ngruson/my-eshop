// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using eShop.Identity.API.Quickstart.Consent;

namespace eShop.Identity.API.Quickstart.Device;

public class DeviceAuthorizationInputModel : ConsentInputModel
{
    public required string UserCode { get; set; }
}
