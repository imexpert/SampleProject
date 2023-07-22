using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SampleProject.Business.DependencyResolvers;
using SampleProject.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using SampleProject.Core.DependencyResolvers;
using SampleProject.Core.Utilities.IoC;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json.Serialization;
using SampleProject.Core.Extensions;

namespace SampleProject.Api.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomMediatR(this IServiceCollection services)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(AutofacBusinessModule));

            services.AddMediatR(assembly);
        }

        public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(AutofacBusinessModule));

            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowOrigin",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddSwaggerGen();

            services.AddTransient<MsSqlLogger>();

            var coreModule = new CoreModule();

            services.AddDependencyResolvers(configuration, new ICoreModule[] { coreModule });

            services.AddAutoMapper(assembly);

            services.AddValidatorsFromAssembly(assembly);

            ValidatorOptions.Global.DisplayNameResolver = (type, memberInfo, expression) =>
            {
                return memberInfo.GetCustomAttribute<DisplayAttribute>()
                    ?.GetName();
            };
        }
    }
}
