using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nancy.Owin;

namespace HelloMicroservices
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseOwin(buildFunc =>
            {
                buildFunc(next => env1 => 
                {
                    Console.WriteLine("GotRequest");
                    return next(env1);

                });
                buildFunc.UseNancy();
            });
        }
    }
}
