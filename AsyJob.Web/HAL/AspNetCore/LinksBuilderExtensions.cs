using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using static AsyJob.Web.HAL.Link;

namespace AsyJob.Web.HAL.AspNetCore
{
    public static class LinkBuilderExtensions
    {
        public static LinkBuilder FromController(this LinkBuilder builder, Type type, string methodName)
        {
            var controllerRoute = GetControllerRoute(type);
            var method = type.GetMethod(methodName) 
                ?? throw new ArgumentException("Method not found", nameof(methodName));
            var methodRoute = GetMethodRoute(method);
            var route = string.Join('/', controllerRoute, methodRoute)
                .Replace("///", "/")
                .Replace("//", "/");
            builder.SetHref(route);
            builder.SetTemplated(IsTemplated(route));
            return builder;
        }

        private static string GetControllerRoute(Type type)
            => type.GetCustomAttribute<RouteAttribute>()?.Template ?? "/";

        private static string GetMethodRoute(MethodInfo method)
        {
            var route = method.GetCustomAttribute<RouteAttribute>();
            if (route is not null)
            {
                return route.Template;
            }
            var httpMethodAttribute = method.GetCustomAttribute<HttpMethodAttribute>();
            return httpMethodAttribute?.Template ?? "/";
        }

        private static bool IsTemplated(string route)
            => route.Contains('{') && route.Contains('}');
    }
}
