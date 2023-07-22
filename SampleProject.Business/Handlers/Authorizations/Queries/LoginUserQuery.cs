using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SampleProject.Business.Handlers.Authorizations.ValidationRules;
using SampleProject.Core.Aspects.Autofac.Validation;
using SampleProject.Core.Utilities.IoC;
using SampleProject.Core.Utilities.Results;
using SampleProject.Entities.Concrete;
using SampleProject.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Business.Handlers.Authorizations.Queries
{
    public class LoginUserQuery : IRequest<ResponseMessage<User>>
    {
        public LoginUserDto LoginModel { get; set; }

        public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, ResponseMessage<User>>
        {
            [ValidationAspect(typeof(LoginUserValidator), Priority = 1)]
            public async Task<ResponseMessage<User>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
            {
                var mapper = ServiceTool.ServiceProvider.GetService<IMapper>();

                var user = mapper.Map<User>(request.LoginModel);

                /*
                 * Some db business code here
                 * normally this method must be asynchronous.
                 */

                return ResponseMessage<User>.Success(user);
            }
        }
    }
}
