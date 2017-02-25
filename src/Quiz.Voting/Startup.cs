using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quiz.EventSourcing;
using Quiz.EventSourcing.Domain;
using Quiz.Messages;
using Swashbuckle.AspNetCore.Swagger;

namespace Quiz.Voting
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
            services.AddMvcCore().AddApiExplorer().AddJsonFormatters();
            services.AddSwaggerGen(c => 
                c.SwaggerDoc("v1", new Info { Title = "Quiz Voting API", Version = "v1" })
            );
            
            AddEventStore(services);
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1")
            );
        }
        
        private void AddEventStore(IServiceCollection services)
        {
            services.AddSingleton(EventStoreConnectionFactory.Create(Configuration["EVENT_STORE"]));
            services.AddSingleton(new EventTypeResolver(ReflectionHelper.MessagesAssembly));
            services.AddTransient<IRepository, EventStoreRepository>();          
        }
    }
}
