namespace Clinic.Initializer
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Clinic.DataAccess;
    using Clinic.Domain;
    using Clinic.Services;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using Newtonsoft.Json;

    public class Program
    {
        private static readonly IConfigurationRoot AppConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        
        public static void Main(string[] args)
        {
            var actions = new Dictionary<string, Action>
                {
                    ["Migrate db"] = MigrateDb,
                    ["Create admin"] = CreateAdmin,
                    ["Run heroku psql"] = RunHerokuPsql
                }
                .ToArray();

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

        private static void RunHerokuPsql()
        {
            Process.Start("CMD.exe", "heroku pg:psql");
        }

        private static void MigrateDb()
        {
            using (var dataContext = GetDataContext())
            {
                dataContext.Database.Migrate();
            }
        }

        private static void CreateAdmin()
        {
            var cryptoService = new CryptoService(JsonConvert.DeserializeObject<byte[]>(AppConfiguration["AccessTokenSymmetricKey"]));
            using (var dataContext = GetDataContext())
            {
                var user = new User
                {
                    UserName = "admin",
                    FirstName = "Администратор",
                    SecondName = "Администратор",
                    ThirdName = "Администратор",
                    Permissions = new[] { UserPermission.All },
                    Phone = 89999999999,
                    PasswordHash = "admin",
                };

                dataContext.Users.Add(user);
                dataContext.SaveChanges();

                var token = cryptoService.Encrypt(user.Id);
                user.PasswordHash = token;
                dataContext.SaveChanges();
            }
        }

        private static DataContext GetDataContext()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseNpgsql(AppConfiguration["DbConnection"]);
            return new DataContext(builder.Options);
        }
    }
}
