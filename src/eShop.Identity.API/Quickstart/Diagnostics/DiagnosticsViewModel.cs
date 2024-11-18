// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Text;
using System.Text.Json;

namespace eShop.Identity.API.Quickstart.Diagnostics;

public class DiagnosticsViewModel
{
    public DiagnosticsViewModel(AuthenticateResult result)
    {
        this.AuthenticateResult = result;

        if (result.Properties?.Items?.TryGetValue("client_list", out string? encoded) == true)
        {
            byte[] bytes = Base64Url.Decode(encoded!);
            string value = Encoding.UTF8.GetString(bytes);

            this.Clients = JsonSerializer.Deserialize<string[]>(value);
        }
    }

    public AuthenticateResult AuthenticateResult { get; }
    public IEnumerable<string>? Clients { get; } = [];
}
