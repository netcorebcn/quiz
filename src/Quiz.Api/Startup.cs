using EasyEventSourcing;
using EasyNetQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quiz.Domain;
using Quiz.Domain.Commands;
using Swashbuckle.AspNetCore.Swagger;
using static Quiz.Api.RetryExtensions;

namespace Quiz.Api
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public ILogger<Startup> Logger { get; }

        public Startup(ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            Configuration = builder.Build();
            Logger = loggerFactory.CreateLogger<Startup>();
            loggerFactory.AddConsole();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddMvcCore().AddApiExplorer().AddJsonFormatters();
            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new Info { Title = "Quiz Voting API", Version = "v1" })
            );

            services.AddEasyEventSourcing(
                EventStoreOptions.Create(
                    Configuration["EVENT_STORE"],
                    Configuration["EVENT_STORE_MANAGER_HOST"],
                    Configuration["STREAM_NAME"]),
                ReflectionHelper.DomainAssembly);

            services.AddSingleton<IBus>(RabbitHutch.CreateBus(Configuration["BUS_CONNECTION"]));
            services.AddTransient<QuizAppService>();
        }

        public void Configure(IApplicationBuilder app, IBus brokerBus, QuizAppService quizAppService)
        {
            app.UseCors("CorsPolicy");
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1")
            );

            DefaultRetry(SubscribeToQuizAnswers);            

            void SubscribeToQuizAnswers () => 
                brokerBus.SubscribeAsync<QuizAnswersCommand>("QuizAnswersCommandSubscription",
                    async message =>
                    {
                        Logger.LogInformation(message.ToString());
                        await quizAppService.Vote(message);
                    });
        }
    }
}
