/*
 * Parcel Logistics Service
 *
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.20.0
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Elsa.SKS.Filters;
using Elsa.SKS.MappingProfiles;
using Elsa.SKS.Package.BusinessLogic;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.BusinessLogic.Validators;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Parcel = Elsa.SKS.Package.BusinessLogic.Entities.Parcel;
using Warehouse = Elsa.SKS.Package.BusinessLogic.Entities.Warehouse;

namespace Elsa.SKS
{
    /// <summary>
    /// Startup
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private readonly IWebHostEnvironment _hostingEnv;

        private IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="env"></param>
        /// <param name="configuration"></param>
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _hostingEnv = env;
            Configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add business layer components
            services.AddTransient<IWarehouseLogic, WarehouseLogic>();
            services.AddTransient<IParcelTrackingLogic, ParcelTrackingLogic>();
            services.AddTransient<IParcelRegistrationLogic, ParcelRegistrationLogic>();
            
            services.AddTransient<IParcelRepository, SqlParcelRepository>();
            services.AddTransient<IHopRepository, SqlHopRepository>();
            services.AddTransient<IAppDbContext, AppDbContext>();
            
            // Add validators
            services.AddTransient<IValidator<Parcel>, ParcelValidator>();
            services.AddTransient<IValidator<Warehouse>, WarehouseValidator>();

            // Add framework services.
            services
                .AddMvc(options =>
                {
                    options.InputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonInputFormatter>();
                    options.OutputFormatters.RemoveType<Microsoft.AspNetCore.Mvc.Formatters.SystemTextJsonOutputFormatter>();
                })
                .AddNewtonsoftJson(opts =>
                {
                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                })
                .AddXmlSerializerFormatters();


            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("1.20.1", new OpenApiInfo
                    {
                        Version = "1.20.1",
                        Title = "Parcel Logistics Service",
                        Description = "Parcel Logistics Service (ASP.NET Core 3.1)",
                        Contact = new OpenApiContact()
                        {
                           Name = "SKS",
                           Url = new Uri("http://www.technikum-wien.at/"),
                           Email = ""
                        },
                        //TermsOfService = new Uri("")
                    });
                    c.CustomSchemaIds(type => type.FullName);
                    c.IncludeXmlComments($"{AppContext.BaseDirectory}{Path.DirectorySeparatorChar}{_hostingEnv.ApplicationName}.xml");

                    // Include DataAnnotation attributes on Controller Action parameters as Swagger validation rules (e.g required, pattern, ..)
                    // Use [ValidateModelState] on Actions to actually validate it in C# as well!
                    c.OperationFilter<GeneratePathParamsValidationFilter>();
                });

            services
                .AddSwaggerGenNewtonsoftSupport();

            services
                .AddAutoMapper(
                    typeof(ParcelProfile).Assembly,
                    typeof(Package.BusinessLogic.MappingProfiles.ParcelProfile).Assembly
                );

            services
                .AddDbContextPool<AppDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("ElsaDbConnection"));
                    options.UseLazyLoadingProxies();
                });

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRouting();

            //TODO: Uncomment this if you need wwwroot folder
            // app.UseStaticFiles();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //TODO: Either use the SwaggerGen generated Swagger contract (generated from C# classes)
                c.SwaggerEndpoint("/swagger/1.20.1/swagger.json", "Parcel Logistics Service");

                //TODO: Or alternatively use the original Swagger contract that's included in the static files
                // c.SwaggerEndpoint("/swagger-original.json", "Parcel Logistics Service Original");
            });

            //TODO: Use Https Redirection
            // app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //TODO: Enable production exception handling (https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling)
                app.UseExceptionHandler("/Error");

                app.UseHsts();
            }
            
        }
    }
}
