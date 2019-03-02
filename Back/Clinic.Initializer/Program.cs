namespace Clinic.Initializer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Clinic.DataAccess;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class Program
    {
        private static readonly IConfigurationRoot AppConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        
        public static void Main(string[] args)
        {
            var actions = new Dictionary<string, Action>().ToArray();

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

        private static DataContext GetDataContext()
        {
            var builder = new DbContextOptionsBuilder<DataContext>();
            builder.UseNpgsql(AppConfiguration["DbConnection"]);
            return new DataContext(builder.Options);
        }
    }
}
