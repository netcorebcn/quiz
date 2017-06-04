using System.Threading.Tasks;
using EasyEventSourcing;
using EasyWebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quiz.Domain;

namespace Quiz.Results
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials() );
            })
            .AddEasyEventSourcing<QuizAggregate>(Configuration)
            .AddEasyWebSockets()
            .AddSingleton<QuizResultsService>();

        public void Configure(IApplicationBuilder app) =>
            app.UseCors("CorsPolicy")
                .UseEasyWebSockets()
                .UseQuizResultsService(); 
    }
}
