using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Reflection;
using System.Text;
using static AsyJob.Web.HAL.Link;

namespace AsyJob.Web.HAL.AspNetCore
{
    public static class LinkBuilderExtensions
    {
        public static LinkBuilder FromController(
            this LinkBuilder builder, 
            Type type, 
            string methodName, 
            Dictionary<string, object>? arguments = null)
        {
            var controllerRoute = GetControllerRoute(type);
            var method = type.GetMethod(methodName) 
                ?? throw new ArgumentException("Method not found", nameof(methodName));
            var methodRoute = GetMethodRoute(method);
            var route = string.Join('/', controllerRoute, methodRoute)
                .Replace("///", "/")
                .Replace("//", "/");
            if(arguments is not null) 
                route = PlaceArguments(route, arguments);
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

        private static string PlaceArguments(string route, Dictionary<string, object> arguments)
        {
            var newRoute = new StringBuilder(route);
            foreach(var argument in arguments) 
            {
                newRoute.Replace($"{{{argument.Key}}}", argument.Value.ToString());
            }
            return newRoute.ToString();
        }

        private static bool IsTemplated(string route)
            => route.Contains('{') && route.Contains('}');
    }
}
