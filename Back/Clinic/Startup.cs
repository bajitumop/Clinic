﻿namespace Clinic
{
    using Clinic.DataAccess;
    using Clinic.DataAccess.Implementations;
    using Clinic.DataAccess.Repositories;
    using Clinic.Middlewares;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

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
            services.AddMvc();
            services.AddTransient<IServicesRepository, ServicesRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<DataContext>().Database.Migrate();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseMvc(routeBuilder => routeBuilder.MapRoute("default", "{controller}/{action}/{id?}"));
        }
    }
}
