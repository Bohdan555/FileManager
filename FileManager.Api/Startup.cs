using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using TransactionManager.Components.Filters;
using TransactionManager.DAL.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using TransactionManager.Components.Infrastructure;
using System.Text.Json.Serialization;

namespace TransactionManager.Api
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
            services.AddDbContext<TransactionManagerContext>(options =>
                options.UseSqlServer(connection));

            services.AddMvc(options =>
            {
                options.Filters.Add<CommonExceptionFilter>();
            })
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddSwaggerGen(c =>
            {
                c.DescribeAllEnumsAsStrings();
                c.DescribeAllParametersInCamelCase();
                c.DescribeStringEnumsInCamelCase();
                c.EnableAnnotations();

                c.SwaggerDoc("v1", new OpenApiInfo());
            });

            services.AddAutoMapper(typeof(TransactionManagerMapperProfile));

            var builder = new ContainerBuilder();

            builder.Populate(services);

            builder.RegisterModule(new TransactionManagerAutofacModule());

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
