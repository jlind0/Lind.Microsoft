using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using Microsoft.AspNetCore.Builder;


namespace Lind.Microsoft.Core.Web
{
    public static class WebHostBuilderExstensions
    {
        public static IServiceCollection AddRouteRegistry<T>(this IServiceCollection collection)
            where T: RouteRegistry, new()
        {
            ServiceDescriptor descriptor = new ServiceDescriptor(typeof(RouteRegistry), new T());
            collection.Add(descriptor);
            return collection;
        }
    }
}
