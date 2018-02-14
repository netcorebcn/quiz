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
using Swashbuckle.AspNetCore.Swagger;

namespace Quiz.Results.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) => services
            .AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new Info { Title = "Quiz Results API", Version = "v1" })
            )
            .AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            })
            .AddQuizResultsApp(Configuration)
            .AddMvc();

        public void Configure(IApplicationBuilder app) => app
            .UseCors("CorsPolicy")
            .UseQuizResultsApp()
            .UseMvc()
            .UseSwagger(c => c.RouteTemplate = "api/swagger/{documentName}/swagger.json")
            .UseSwaggerUI(c => {
                c.RoutePrefix = "api/swagger";
                c.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Quiz Results API v1");
                }
            );
    }
}
