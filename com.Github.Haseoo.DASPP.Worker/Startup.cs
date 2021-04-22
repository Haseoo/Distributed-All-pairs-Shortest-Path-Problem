using com.Github.Haseoo.DASPP.Worker.CoreData.Dtos;
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
            services.AddControllers();
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

        private void RegisterSelf(IApplicationBuilder app, ILogger<Startup> logger)
        {
            var hostInfo = new WorkerHostInfo()
            {
                Uri = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First(),
                CoreCount = Environment.ProcessorCount
            };
            var client = new RestClient();
            var request = new RestRequest(Configuration["RegisterUri"]);
            request.AddJsonBody(hostInfo);
            var response = client.Post(request);
            if (!response.IsSuccessful)
            {
                logger.LogError($"Could not register worker: {response.StatusCode} {response.StatusDescription}");
               // Environment.Exit(-1);
            }
            else
            {
                logger.LogInformation("Successfully registered!");
            }
        }

        private void DeregisterSelf(IApplicationBuilder app, ILogger<Startup> logger)
        {
            var uri = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses.First();
            var client = new RestClient();
            var request = new RestRequest(Configuration["RegisterUri"]);
            request.AddOrUpdateParameter("uri", uri);
            var response = client.Delete(request);
            if (!response.IsSuccessful)
            {
                logger.LogError($"Could not deregister worker: {response.StatusCode} {response.StatusDescription}");
            }
            else
            {
                logger.LogInformation("Successfully deregistered!");
            }
        }
    }
}