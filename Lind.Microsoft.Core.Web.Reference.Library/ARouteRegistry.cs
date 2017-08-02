using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace Lind.Microsoft.Core.Web.Reference.Library
{
    public class ARouteRegistry : RouteRegistry
    {
        public override void Map(IApplicationBuilder builder)
        {
            builder.UseMvc(routes =>
            {
                routes.MapRoute(name: "home-a", template: "ToThePower/{bse:int}/{power:int}",
                    defaults: new { controller = "A", action = "ToThePower" });
            });
        }
    }
}
