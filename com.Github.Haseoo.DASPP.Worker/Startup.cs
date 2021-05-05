using com.Github.Haseoo.DASPP.CoreData.Dtos;
using com.Github.Haseoo.DASPP.Worker.Infrastructure.Middleware;
using com.Github.Haseoo.DASPP.Worker.Infrastructure.Service;
using com.Github.Haseoo.DASPP.Worker.Providers.Service;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Linq;
using Constants = com.Github.Haseoo.DASPP.CoreData.Constants;

namespace com.Github.Haseoo.DASPP.Worker
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
            services.AddSingleton<ITaskService, TaskService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IHostApplicationLifetime lifetime,
            ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            lifetime.ApplicationStarted.Register(() =>
            {
                RegisterSelf(app, logger);
            });
            lifetime.ApplicationStopping.Register(() =>
            {
                DeregisterSelf(app, logger);
            });
        }

        private void RegisterSelf(IApplicationBuilder app, ILogger logger)
        {
            var hostInfo = new WorkerHostInfo()
            {
                Uri = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First(),
                CoreCount = Environment.ProcessorCount
            };
            var client = new RestClient();
            client.AddDefaultHeader(Constants.ApiKeyName, Configuration["ApiKey"]);
            var request = new RestRequest(Configuration["RegisterUri"]);
            request.AddJsonBody(hostInfo);
            var response = client.Post(request);
            if (!response.IsSuccessful)
            {
                logger?.LogError($"Could not register worker: {response.StatusCode} {response.StatusDescription}");
                // Environment.Exit(-1);
            }
            else
            {
                logger?.LogInformation("Successfully registered!");
            }
        }

        private void DeregisterSelf(IApplicationBuilder app, ILogger logger)
        {
            var uri = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First();
            var client = new RestClient();
            client.AddDefaultHeader(Constants.ApiKeyName, Configuration["ApiKey"]);
            var request = new RestRequest(Configuration["RegisterUri"]);
            request.AddOrUpdateParameter("uri", uri);
            var response = client.Delete(request);
            if (!response.IsSuccessful)
            {
                logger?.LogError($"Could not deregister worker: {response.StatusCode} {response.StatusDescription}");
            }
            else
            {
                logger?.LogInformation("Successfully deregistered!");
            }
        }
    }
}