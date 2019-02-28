namespace Clinic
{
    using System;

    using Clinic.DataAccess;
    using Clinic.DataAccess.Implementations;
    using Clinic.DataAccess.Repositories;
    using Clinic.Middlewares;
    using Clinic.Services;
    using Clinic.Support.Filters;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
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
            if (!this.environment.IsProduction())
            {
                connectionString = this.appConfiguration.GetConnectionString("DbConnection");
            }
            else
            {
                var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                var databaseUri = new Uri(databaseUrl);
                var userInfo = databaseUri.UserInfo.Split(':');

                var builder = new NpgsqlConnectionStringBuilder
                                  {
                                      Host = databaseUri.Host,
                                      Port = databaseUri.Port,
                                      Username = userInfo[0],
                                      Password = userInfo[1],
                                      Database = databaseUri.LocalPath.TrimStart('/')
                                  };

                connectionString = builder.ToString();
            }

            services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

            services.AddMvc(options =>
                {
                    options.Filters.Add<UserPermissionFilter>();
                    options.Filters.Add(typeof(ModelValidationFilterAttribute));
                });

            services.AddTransient<IServicesRepository, ServicesRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IImagesRepository, ImagesRepository>();
            services.AddTransient<DatabaseInitializer>();
            services.AddSingleton(new CryptoService(JsonConvert.DeserializeObject<byte[]>(this.appConfiguration["AccessTokenSymmetricKey"])));

            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "Clinic API", Version = "v1" }); });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMvc(routeBuilder => routeBuilder.MapRoute("default", "{controller}/{action}/{id?}"));
        }
    }
}
