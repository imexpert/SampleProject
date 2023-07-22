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

namespace SampleProject.Business.Handlers.Authorizations.Commands
{
    public class RegisterUserCommand : IRequest<ResponseMessage<User>>
    {
        public UserDto Model { get; set; }
        public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ResponseMessage<User>>
        {
            [ValidationAspect(typeof(RegisterUserValidator), Priority = 1)]
            public async Task<ResponseMessage<User>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            {
                var mapper = ServiceTool.ServiceProvider.GetService<IMapper>();

                var user = mapper.Map<User>(request.Model);

                /*
                 * Some db business code here
                 * normally this method must be asynchronous.
                 */

                return ResponseMessage<User>.Success(user);
            }
        }
    }
}
