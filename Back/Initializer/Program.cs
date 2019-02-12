namespace Initializer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Program
    {
        public static void Main(string[] args)
        {
            var actions = new Dictionary<string, Action>
                {
                    ["Create init file"] = CreateInitFile
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

        private static void CreateInitFile()
        {
            Console.WriteLine("CreateInitFile");
        }
    }
}
