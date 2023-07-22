using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SampleProject.Core.Utilities.IoC;
using SampleProject.Core.Utilities.Results;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleProject.Core.Extensions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;


        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }


        private async Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            var environment = ServiceTool.ServiceProvider.GetService<IWebHostEnvironment>();

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            string message;

            message = e.GetInnermostException().Message;

            if (e.GetType() == typeof(ValidationException))
            {
                var vex = (ValidationException)e;
                message = string.Join(", ", vex.Errors.Select(s => s.ErrorMessage).ToArray());
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (e.GetType() == typeof(ApplicationException))
            {
                message = e.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (e.GetType() == typeof(UnauthorizedAccessException))
            {
                message = e.Message;
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else if (e.GetType() == typeof(SecurityException))
            {
                message = e.Message;
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else if (e.GetType() == typeof(NotSupportedException))
            {
                message = e.Message;
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                message = e.GetInnermostException().Message;
            }

            var response = ResponseMessage<NoContent>.Fail(httpContext.Response.StatusCode, message);

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
