using com.Github.Haseoo.DASPP.CoreData;
using com.Github.Haseoo.DASPP.Main.Infrastructure.Middleware;
using com.Github.Haseoo.DASPP.Main.Infrastructure.Service;
using com.Github.Haseoo.DASPP.Main.Providers.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using com.Github.Haseoo.DASPP.CoreData.Dtos;
using FluentValidation.AspNetCore;

namespace com.Github.Haseoo.DASPP.Main
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddFluentValidation(s => s.RegisterValidatorsFromAssemblyContaining<GraphDto>());
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = Configuration["ProjectTitle"], Version = "v1" });

                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme()
                {
                    Name = Constants.ApiKeyName,
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Authorization by x-api-key inside request's header",
                    Scheme = "ApiKeyScheme"
                });

                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    },
                    In = ParameterLocation.Header
                };
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { key, new List<string>() }
                });
            });
            services.AddSingleton<IWorkerHostService, WorkerHostService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/v1/swagger.json", Configuration["ProjectTitle"]));

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}