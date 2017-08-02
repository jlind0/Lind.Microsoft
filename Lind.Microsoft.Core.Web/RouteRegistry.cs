using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;

namespace Lind.Microsoft.Core.Web
{
    public abstract class RouteRegistry
    {

        public abstract void Map(IApplicationBuilder builder);
    }
}
