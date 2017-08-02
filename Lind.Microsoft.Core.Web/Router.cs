using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Lind.Microsoft.Core.Web
{
    public static class Router
    {
        public static void MapRegistry(this IApplicationBuilder builder)
        {
            foreach (var reg in builder.ApplicationServices.GetServices<RouteRegistry>())
                reg.Map(builder);
        }
    }
}
