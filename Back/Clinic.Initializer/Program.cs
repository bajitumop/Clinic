namespace Clinic.Initializer
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Dapper;
    using DataAccess.Implementations;
    using DataAccess.Repositories;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Migrations;
    using Npgsql;

    public class Program
    {
        private static readonly IConfigurationRoot AppConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        private static IServiceProvider serviceProvider;

        public static void Main(string[] args)
        {
            var connectionString = AppConfiguration.GetConnectionString("DbConnection");

            serviceProvider = new ServiceCollection()
                .AddTransient<IDbConnection>(provider => new NpgsqlConnection(connectionString))
                .AddTransient<IServicesRepository, ServicesRepository>()
                .AddTransient<IUsersRepository, UsersRepository>()
                .AddTransient<IImagesRepository, ImagesRepository>()
                .AddTransient<IDoctorsRepository, DoctorsRepository>()
                .AddTransient<ISchedulesRepository, SchedulesRepository>()
                .AddTransient<IVisitsRepository, VisitsRepository>()
                .BuildServiceProvider();

            var actions = new Dictionary<string, Action>
            {
                ["Migrate database"] = MigrateDatabase
            }.ToArray();

            var choice = args.Any() ? args[0] : null;
            if (choice == null)
            {
                for (var i = 0; i < actions.Length; i++)
                {
                    Console.WriteLine($"{i}) {actions[i].Key}");
                }

                Console.Write("Select your choice: ");
                choice = Console.ReadLine();
            }

            var action = actions[int.Parse(choice)].Value;

            try
            {
                action();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }

        private static T GetService<T>()
        {
            return (T) serviceProvider.GetService(typeof(T));
        }
        
        public static void MigrateDatabase()
        {
            var connection = GetService<IDbConnection>();
            try
            {
                connection.Open();
            }
            catch (Exception)
            {
                try
                {
                    var connectionString = AppConfiguration.GetConnectionString("DbConnection");
                    var initConnectionString = Regex.Replace(
                        connectionString,
                        "Database=.*?;", 
                        string.Empty, 
                        RegexOptions.IgnoreCase);

                    var initConnection = new NpgsqlConnection(initConnectionString);
                    initConnection.Open();
                    initConnection.Execute($"CREATE DATABASE \"{connection.Database}\" WITH ENCODING = 'UTF8'");
                    connection.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Could not create database by default {ex}");
                    return;
                }
            }

            var appliedMigrations = new HashSet<string>();
            try
            {
                appliedMigrations = connection.Query<string>(@"select ""Name"" from _migrations").ToHashSet();
            }
            catch (PostgresException)
            {
                try
                {
                    connection.Execute(@"create table _migrations (
	                    ""Name"" varchar primary key,
	                    ""Type"" varchar not null,
	                    ""ExecutedOn"" timestamp not null,
	                    ""ExecutedBy"" varchar not null
                    );");
                }
                catch (Exception exc)
                {
                    Console.WriteLine($"Could not create migration log by default. {exc}");
                    return;
                }
            }

            var migrations = typeof(Migration).Assembly.GetTypes()
                .Where(type => !type.IsAbstract && typeof(Migration).IsAssignableFrom(type))
                .Select(type => (Migration) Activator.CreateInstance(type));

            foreach (var migration in migrations.Where(m => !appliedMigrations.Contains(m.GetType().Name)).OrderBy(m => m.MigrationDateTime))
            {
                try
                {
                    Console.WriteLine($"Applying migration: {migration.GetType().Name}");
                    var transaction = connection.BeginTransaction();
                    migration.ExecuteAsync(connection).Wait();
                    connection.Execute(
                        $@"insert into _migrations values (@Name, @Type, TIMESTAMP '{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}', 'Initializer')",
                        new { migration.GetType().Name, Type = migration is BaseScriptMigration ? "script" : "code" });
                    transaction.Commit();
                    Console.WriteLine($"Applied migration: {migration.GetType().Name}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Patch {migration.GetType().Name} failed", ex);
                }
            }
        }
    }
}
