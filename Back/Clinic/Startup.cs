﻿namespace Clinic
{
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

    public class Startup
    {
        private readonly IConfigurationRoot appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            this.appConfiguration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = this.appConfiguration.GetConnectionString("DbConnection");
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
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMvc(routeBuilder => routeBuilder.MapRoute("default", "{controller}/{action}/{id?}"));
        }
    }
}
