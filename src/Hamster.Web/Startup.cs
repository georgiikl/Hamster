using Autofac;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Hamster.Web.Extensions;

namespace Hamster.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddControllersAsServices();
            services.AddSwaggerGenerator();
            services.AddMediatR(o => { }, typeof(Hamster.UseCases.DependencyInjection));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.ConfigureContainer();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwaggerMiddlewares();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            { 
                endpoints.MapControllers();
            });
        }
    }
}