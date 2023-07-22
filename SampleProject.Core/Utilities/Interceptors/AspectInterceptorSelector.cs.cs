using Castle.DynamicProxy;
using SampleProject.Core.Aspects.Autofac.Exception;
using SampleProject.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Core.Utilities.Interceptors
{
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(true).ToList();
            var methodAttributes =
                type.GetMethod(method.Name)?.GetCustomAttributes<MethodInterceptionBaseAttribute>(true);
            if (methodAttributes != null)
            {
                classAttributes.AddRange(methodAttributes);
            }

            //Default olarak çalışmasını istediğiniz aspect varsa bu kısma ekleyebilirsiniz.
            classAttributes.Add(new ExceptionLogAspect(typeof(MsSqlLogger)));

            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}
