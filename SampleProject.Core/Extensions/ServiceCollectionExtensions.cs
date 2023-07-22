using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleProject.Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDependencyResolvers(this IServiceCollection services, IConfiguration configuration, ICoreModule[] modules)
        {
            foreach (var module in modules)
            {
                module.Load(services, configuration);
            }
        }
    }
}
