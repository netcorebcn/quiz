using System;
using EasyEventSourcing;
using EasyEventSourcing.Aggregate;
using EasyNetQ;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Quiz.Domain;
using Quiz.Domain.Commands;
using Swashbuckle.AspNetCore.Swagger;

namespace Quiz.Api
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(ILoggerFactory loggerFactory)
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();
            Configuration = builder.Build();
            loggerFactory.AddConsole();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials() );
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
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            ILoggerFactory loggerFactory,
            IRepository quizRepository,
            IBus brokerBus)
        {
            app.UseCors("CorsPolicy");
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1")
            );

            var logger = loggerFactory.CreateLogger<Startup>();     

            Policy.Handle<Exception>()
            .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            .Execute(() => 
                brokerBus.SubscribeAsync<QuizAnswersCommand>("QuizAnswersCommandSubscription", 
                    async message => {
                        logger.LogInformation(message.ToString());

                        var quiz = await quizRepository.GetById<QuizAggregate>(message.QuizId);
                        message.Answers.ForEach(answer =>
                            quiz.Vote(answer.QuestionId, answer.OptionId)
                        );
                        await quizRepository.Save(quiz);
                    })
            );
        }        
    }
}
