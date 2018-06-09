using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quiz.Api.EventStore;
using Quiz.Domain;
using Quiz.Domain.Commands;

namespace Quiz.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) => services
            .AddEventStore(Configuration)
            .AddSingleton<IBus>(RabbitHutch.CreateBus(Configuration["messagebroker"].Trim() ?? "amqp://guest:guest@localhost:5672"))
            .AddTransient<QuizAppService>()
            .AddMvc();

        public void Configure(IApplicationBuilder app) => app
            .UseMvc();
    }
}
