using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quiz.Api.EventStore;
using Quiz.Domain;
using Quiz.Domain.Commands;
using Swashbuckle.AspNetCore.Swagger;
using EasyWebSockets;

namespace Quiz.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) => services
            .AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new Info { Title = "Quiz Voting API", Version = "v1" })
            )
            .AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            })
            .AddEventStore(Configuration)
            .AddEasyWebSockets()
            .AddTransient<QuizAppService>()
            .AddTransient<QuizResultsAppService>()
            .AddMvc();

        public void Configure(IApplicationBuilder app) => app
            .UseCors("CorsPolicy")
            .UseEasyWebSockets()
            .UseMvc()
            .UseSwagger()
            .UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1")
            );
    }
}
