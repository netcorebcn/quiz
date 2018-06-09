using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using EasyWebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quiz.Results.Api.EventStore;

namespace Quiz.Results.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) => services
            .AddEventStore(Configuration)
            .AddQuizResultsApp(Configuration)
            .AddMvc();

        public void Configure(IApplicationBuilder app) => app
            .UseQuizResultsApp()
            .UseMvc();
    }
}
