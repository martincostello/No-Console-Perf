// Copyright (c) Martin Costello, 2019. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace NoConsolePerf
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting()
               .UseEndpoints(routes =>
               {
                   routes.MapGet(
                       "/",
                       async (context) =>
                       {
                           context.RequestServices.GetRequiredService<ILogger<Startup>>().LogInformation("Processed request");
                           await context.Response.Body.WriteAsync(Array.Empty<byte>());
                       });
               });
        }
    }
}
