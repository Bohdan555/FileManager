using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FileManager.Components.Filters;
using FileManager.Components.Infrastructure;
using FileManager.DAL.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace WebApplication2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            var connection = Configuration.GetConnectionString("DefaultConnection");            
            services.AddDbContext<FileManagerContext>(options =>
                options.UseSqlServer(connection));

            services.AddMvc(options =>
            {
                 options.Filters.Add<CommonExceptionFilter>();
            });

            services.AddSwaggerGen(c =>
            {                
                c.DescribeAllEnumsAsStrings();
                c.DescribeAllParametersInCamelCase();
                c.DescribeStringEnumsInCamelCase();
                c.EnableAnnotations();                

                c.SwaggerDoc("v1", new OpenApiInfo());                                
            });

            services.AddAutoMapper(typeof(FileManagerMapperProfile));

            var builder = new ContainerBuilder();

            builder.Populate(services);

            builder.RegisterModule(new FileManagerAutofacModule());

            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {            
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "File Service");
                c.RoutePrefix = string.Empty;               
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
