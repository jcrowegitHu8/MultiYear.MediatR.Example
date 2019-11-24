using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

using core = BusinessLogic.Core;
using y1 = BusinessLogic.Year1;
using Swashbuckle.AspNetCore.Swagger;

namespace MultiYear.MediatR.Example.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            
            ConfigureSwagger(services);
            return ConfigureDependencyInjection(services);
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MultiYear MediatR API V1");
                })
                .UseMvc();
        }

        private IServiceProvider ConfigureDependencyInjection(IServiceCollection services)
        {
            var assemblies = EnsureAlAllAssembliesHaveLoadedToScan(
                Assembly.GetExecutingAssembly(),
                new List<Type>
                {
                    typeof(y1.DI.Year2Module),
                    typeof(core.DI.CoreModule),
                });


            var builder = new ContainerBuilder();

            builder.Populate(services);
            builder.RegisterAssemblyModules(assemblies);
            builder.AddMediatR(assemblies);

            var appContainer = builder.Build();
            return new AutofacServiceProvider(appContainer);
        }

        public Assembly[] EnsureAlAllAssembliesHaveLoadedToScan(
            Assembly currentAssembly,
            List<Type> dependentyTypesToLoad)
        {
            foreach (var type in dependentyTypesToLoad)
            {
                Activator.CreateInstance(type);
            }

            var assemblieNames = currentAssembly.GetReferencedAssemblies().Where(o => o.Name.StartsWith("BusinessLogic")).ToArray();
            var assemblies = new List<Assembly>();

            foreach (var name in assemblieNames)
            {
                assemblies.Add(AppDomain.CurrentDomain.Load(name));
            }

            assemblies.Add(currentAssembly);

            return assemblies.ToArray();
        }

        private void ConfigureSwagger(IServiceCollection services)
        {
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlFile);
            services.AddMvcCore().AddApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "MultiYear MediatR Example API", Version = "v1" });
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
