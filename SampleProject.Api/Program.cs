using Autofac;
using Autofac.Extensions.DependencyInjection;
using SampleProject.Api.Infrastructure;
using SampleProject.Business.DependencyResolvers;
using SampleProject.Core.Utilities.IoC;
using Swashbuckle.AspNetCore.SwaggerUI;
using SampleProject.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

//Custom Services
builder.Services.AddCustomServices(builder.Configuration);

//add basic MVC feature
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    );

builder.Services.AddCustomMediatR();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterModule(new AutofacBusinessModule()));

var app = builder.Build();

// This should be called before any other middleware
app.UseForwardedHeaders();

// VERY IMPORTANT. Since we removed the build from AddDependencyResolvers, let's set the Service provider manually.
// By the way, we can construct with DI by taking type to avoid calling static methods in aspects.
ServiceTool.ServiceProvider = app.Services;

app.ConfigureCustomExceptionMiddleware();

//await app.UseDbOperationClaimCreator();

// Configure the HTTP request pipeline.
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "Sample Project");
    c.DocExpansion(DocExpansion.None);
});

app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.Use(async (httpContext, next) =>
{
    await next.Invoke();
});

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();