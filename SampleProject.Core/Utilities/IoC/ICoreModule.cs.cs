﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleProject.Core.Utilities.IoC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection services, IConfiguration configuration);
    }
}
