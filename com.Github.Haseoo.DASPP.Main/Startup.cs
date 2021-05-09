using com.Github.Haseoo.DASPP.CoreData;
using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.Main.Dtos.Validators;
using com.Github.Haseoo.DASPP.Main.Infrastructure.Middleware;
using com.Github.Haseoo.DASPP.Main.Infrastructure.Service;
using com.Github.Haseoo.DASPP.Main.Providers.Service;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RestSharp.Serialization.Json;
using System.Collections.Generic;

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
                .AddFluentValidation(s =>
                    s.RegisterValidatorsFromAssemblyContaining<GraphDto>()
                        .RegisterValidatorsFromAssemblyContaining<MainTaskRequestDtoValidator>());
            services.AddMvc();
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
            services.AddSingleton(new JsonDeserializer());
            services.AddSingleton<IWorkerHostService, WorkerHostService>();
            services.AddSingleton<IGraphService, GraphService>();
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AnyOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                    });
            });
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

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}