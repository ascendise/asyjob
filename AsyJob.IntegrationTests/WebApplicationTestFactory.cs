using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AsyJob.IntegrationTests
{
    internal class WebApplicationTestFactory<T> : WebApplicationFactory<T> where T : class    
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Tests");
            var rootDir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            builder.ConfigureAppConfiguration(
                cfg => cfg.AddJsonFile(Path.Combine(rootDir!, "appsettings.Tests.json")));
            builder.ConfigureHostConfiguration(
                cfg => cfg.AddJsonFile(Path.Combine(rootDir!, "appsettings.Tests.json")));
            return base.CreateHost(builder);
        }
    }
}
