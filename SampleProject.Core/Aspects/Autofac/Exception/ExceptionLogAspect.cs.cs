using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SampleProject.Core.CrossCuttingConcerns.Logging.Serilog;
using SampleProject.Core.CrossCuttingConcerns.Logging;
using SampleProject.Core.Utilities.Interceptors;
using SampleProject.Core.Utilities.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using SampleProject.Core.Utilities.Messages;
using Microsoft.Extensions.DependencyInjection;

namespace SampleProject.Core.Aspects.Autofac.Exception
{
    /// <summary>
    /// ExceptionLogAspect
    /// </summary>
    public class ExceptionLogAspect : MethodInterception
    {
        private readonly LoggerServiceBase _loggerServiceBase;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExceptionLogAspect(Type loggerService)
        {
            if (loggerService.BaseType != typeof(LoggerServiceBase))
            {
                throw new ArgumentException(AspectMessages.WrongLoggerType);
            }
            _loggerServiceBase = (LoggerServiceBase)ServiceTool.ServiceProvider.GetService(loggerService);
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        protected override void OnException(IInvocation invocation, System.Exception e)
        {
            var logDetailWithException = GetLogDetail(invocation);

            if (e is not ValidationException)
            {
                logDetailWithException.ExceptionMessage = e is System.Exception
                 ? string.Join(Environment.NewLine, (e as AggregateException).InnerExceptions.Select(x => x.Message))
                 : e.Message;
                _loggerServiceBase.Error(JsonConvert.SerializeObject(logDetailWithException));
            }
        }

        private LogDetailWithException GetLogDetail(IInvocation invocation)
        {
            var logParameters = invocation.Arguments.Select((t, i) => new LogParameter
            {
                Name = invocation.GetConcreteMethod().GetParameters()[i].Name,
                Value = t,
                Type = t.GetType().Name
            })
                .ToList();
            var logDetailWithException = new LogDetailWithException
            {
                MethodName = invocation.Method.Name,
                Parameters = logParameters,
                User = (_httpContextAccessor.HttpContext == null ||
                        _httpContextAccessor.HttpContext.User.Identity.Name == null)
                    ? "?"
                    : _httpContextAccessor.HttpContext.User.Identity.Name
            };
            return logDetailWithException;
        }
    }
}
