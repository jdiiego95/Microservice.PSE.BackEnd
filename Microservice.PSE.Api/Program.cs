using System.Configuration;
using Microservice.PSE.Api.Clients;
using Microservice.PSE.Api.Clients.Interfaces;
using Microservice.PSE.Api.Entities;
using Microservice.PSE.Api.Exceptions;
using Microservice.PSE.Api.Properties;
using Microservice.PSE.Api.Services;
using Microservice.PSE.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace Microservice.PSE.Api
{
    /// <summary>
    /// The main program class for PSE API.
    /// </summary>
    internal class Program
    {
        protected Program() { }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            SetBuilderConfiguration(builder);
            SetAppConfiguration(builder);
        }

        /// <summary>
        /// Configures the builder services.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        public static void SetBuilderConfiguration(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            ConfigureSwagger(builder);

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            ConfigureConnectionDataBase(builder);
            ConfigureServices(builder);
            ConfigureClients(builder);
            ConfigureCorsPolicy(builder);
        }

        /// <summary>
        /// Configures the application pipeline.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        public static void SetAppConfiguration(WebApplicationBuilder builder)
        {
            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PSE API V1");
                    c.DocumentTitle = "PSE Microservice API Documentation";
                });
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsOrigins");
            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }

        /// <summary>
        /// Configures Swagger documentation.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PSE Microservice API",
                    Version = "v1",
                    Description = "API para gestión de bancos y operaciones PSE (Pagos Seguros en Línea)"
                });

                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
        }

        /// <summary>
        /// Configures the database connection.
        /// </summary>
        /// <param name="builder">The host application builder.</param>
        public static void ConfigureConnectionDataBase(IHostApplicationBuilder builder)
        {
            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new DatabaseConfigurationException();

            builder.Services.AddDbContext<MainContext>(options =>
                options.UseSqlServer(connectionString));
        }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IBankService, BankService>();
            // Agregar otros servicios aquí según sea necesario
            // builder.Services.AddTransient<ITransactionService, TransactionService>();
            // builder.Services.AddTransient<IPaymentService, PaymentService>();
        }

        /// <summary>
        /// Configures the clients for the application.
        /// </summary>
        /// <param name="builder">The web application builder.</param>
        private static void ConfigureClients(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IBankClient>(provider =>
            {
                var context = provider.GetRequiredService<MainContext>();
                return new BankClient(context);
            });

            // Agregar otros clientes aquí según sea necesario
            // builder.Services.AddTransient<ITransactionClient>(provider =>
            // {
            //     var context = provider.GetRequiredService<MainContext>();
            //     return new TransactionClient(context);
            // });

            // builder.Services.AddTransient<IPaymentClient>(provider =>
            // {
            //     var context = provider.GetRequiredService<MainContext>();
            //     return new PaymentClient(context);
            // });
        }

        /// <summary>
        /// Configures the CORS policy for the application.
        /// </summary>
        /// <param name="builder">The host application builder.</param>
        private static void ConfigureCorsPolicy(IHostApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsOrigins", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }
    }
}