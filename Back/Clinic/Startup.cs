namespace Clinic
{
    using System;
    using System.Data;
    using DataAccess.Implementations;
    using DataAccess.Repositories;
    using Infrastructure.Routing;
    using Mappings;
    using Middlewares;
    using Services;
    using Support.Filters;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json;

    using Npgsql;

    using Swashbuckle.AspNetCore.Swagger;

    public class Startup
    {
        private readonly IHostingEnvironment environment;
        private readonly IConfigurationRoot appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            this.environment = env;
            this.appConfiguration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString;
            connectionString = this.appConfiguration.GetConnectionString("DbConnection");
            services.AddMvc(options =>
                {
                    options.Filters.Add<AuthenticationFilter>();
                    options.Filters.Add<AuthorizationFilter>();
                    options.Filters.Add<MustBeAuthorizedFilter>();
                    options.Filters.Add<ModelValidationFilterAttribute>();
                    options.Conventions.Insert(0, new RouteConvention(new RouteAttribute("api")));
                });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddTransient<IDbConnection>(serviceProvider => new NpgsqlConnection(connectionString));
            services.AddTransient<IServicesRepository, ServicesRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IImagesRepository, ImagesRepository>();
            services.AddTransient<IDoctorsRepository, DoctorsRepository>();
            services.AddTransient<ISchedulesRepository, SchedulesRepository>();
            services.AddTransient<IVisitsRepository, VisitsRepository>();
            services.AddTransient<ScheduleService>();
            services.AddTransient<DatabaseInitializer>();
            services.AddSingleton(new CryptoService(JsonConvert.DeserializeObject<byte[]>(this.appConfiguration["AccessTokenSymmetricKey"])));

            services.AddSingleton(MapperBuilder.Build());

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Clinic API", Version = "v1" }); });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMvc();
        }
    }
}
