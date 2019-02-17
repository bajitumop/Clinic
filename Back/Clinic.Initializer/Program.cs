namespace Clinic.Initializer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;

    using AngleSharp.Html.Dom;
    using AngleSharp.Html.Parser;

    using Clinic.DataAccess;
    using Clinic.Domain;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class Program
    {
        private static readonly IConfigurationRoot AppConfiguration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        
        public static void Main(string[] args)
        {
            var actions = new Dictionary<string, Action>
                {
                    ["Init data"] = InitData
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

        private static void InitData()
        {
            var htmlParser = new HtmlParser();
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("Windows-1251") })
            {
                var doctors = ParseDoctors(webClient, htmlParser);
                var services = ParseServices(webClient, htmlParser);
                var specialties = services.Select(x => x.Specialty).Distinct().ToArray();
                var scheduleParsingModels = ParseScheduleParsingModels(webClient, htmlParser);

                var schedules = new List<Schedule>();
                foreach (var scheduleParsingModel in scheduleParsingModels)
                {
                    var specialty = specialties.FirstOrDefault(x => x.Name == scheduleParsingModel.SpecialtyName);

                    var doctor = doctors.FirstOrDefault(
                        d => new[] { d.FirstName, d.SecondName, d.ThirdName }.Intersect(scheduleParsingModel.DoctorName).Count()
                             == scheduleParsingModel.DoctorName.Length);

                    if (doctor == null)
                    {
                        Console.WriteLine("Doctor not found by schedule!");
                        continue;
                    }

                    if (specialty == null)
                    {
                        Console.WriteLine("Specialty not found by schedule!");
                        continue;
                    }

                    var schedule = new Schedule
                        {
                            Doctor = doctor,
                            Specialty = specialty,
                            MondayStart = scheduleParsingModel.ScheduleByDayOfWeek[DayOfWeek.Monday]?[0],
                            MondayEnd = scheduleParsingModel.ScheduleByDayOfWeek[DayOfWeek.Monday]?[1],
                            TuesdayStart = scheduleParsingModel.ScheduleByDayOfWeek[DayOfWeek.Tuesday]?[0],
                            TuesdayEnd = scheduleParsingModel.ScheduleByDayOfWeek[DayOfWeek.Tuesday]?[1],
                            WednesdayStart = scheduleParsingModel.ScheduleByDayOfWeek[DayOfWeek.Wednesday]?[0],
                            WednesdayEnd = scheduleParsingModel.ScheduleByDayOfWeek[DayOfWeek.Wednesday]?[1],
                            ThursdayStart = scheduleParsingModel.ScheduleByDayOfWeek[DayOfWeek.Thursday]?[0],
                            ThursdayEnd = scheduleParsingModel.ScheduleByDayOfWeek[DayOfWeek.Thursday]?[1],
                            FridayStart = scheduleParsingModel.ScheduleByDayOfWeek[DayOfWeek.Friday]?[0],
                            FridayEnd = scheduleParsingModel.ScheduleByDayOfWeek[DayOfWeek.Friday]?[1],
                            SaturdayStart = scheduleParsingModel.ScheduleByDayOfWeek[DayOfWeek.Saturday]?[0],
                            SaturdayEnd = scheduleParsingModel.ScheduleByDayOfWeek[DayOfWeek.Saturday]?[1]
                        };

                    schedules.Add(schedule);
                    specialty.Schedules.Add(schedule);
                    doctor.Schedules.Add(schedule);
                }

                DownloadDoctorImages(webClient, doctors, AppConfiguration["ImageFolder"]);

                Console.WriteLine("Inserting data to db...");
                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                optionsBuilder.UseNpgsql(AppConfiguration["DbConnection"]);

                using (var db = new DataContext(optionsBuilder.Options))
                {
                    db.Doctors.AddRange(doctors.Where(x => x.Schedules.Any()));
                    db.Specialties.AddRange(specialties);
                    db.Services.AddRange(services);
                    db.Schedules.AddRange(schedules);
                    db.SaveChanges();
                }

                Console.WriteLine("Statistics:");
                Console.WriteLine($"Total doctors: {doctors.Length}");
                Console.WriteLine($"Doctors with schedule and specialties: {doctors.Count(x => x.Schedules.Any())}");
                Console.WriteLine($"Doctor images loaded: {doctors.Count(x => x.ImageUrl != null)}");

                Console.WriteLine($"Specialties imported: {specialties.Length}");

                Console.WriteLine($"Schedules imported: {schedules.Count}");
                
                Console.WriteLine($"Services loaded: {services.Length}");
                Console.WriteLine("Services by specialty:");
                foreach (var serviceGroup in services.GroupBy(x => x.Specialty))
                {
                    Console.WriteLine($"\t{serviceGroup.Key.Name}: {serviceGroup.Count()}");
                }
            }
        }
        
        private static ScheduleParsingModel[] ParseScheduleParsingModels(WebClient webClient, HtmlParser htmlParser)
        {
            Console.WriteLine("Downloading schedule page...");
            var schedulePage = webClient.DownloadString("http://www.ks-klinika.ru/raspisanie");

            Console.WriteLine("Parsing schedule page...");
            var scheduleDocument = htmlParser.ParseDocument(schedulePage);

            return scheduleDocument.QuerySelectorAll("td")
                .Last(x => x.TextContent.TrimStart().StartsWith("Расписание приёма"))
                .QuerySelectorAll("tr")
                .Where(row => row.TextContent.Trim().Any())
                .Select(row =>
                    {
                        var specialtyName = row.Children[0].TextContent.Trim();
                        var doctorName = row.Children[1].TextContent.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);

                        var schedule = row.Children[3].TextContent.Split('\n').Skip(1).Take(3)
                            .Select(x => new string(x.Where(char.IsDigit).ToArray()))
                            .Concat(row.Children[5].TextContent.Split('\n').Skip(1).Take(3)
                                    .Select(x => new string(x.Where(char.IsDigit).ToArray())))
                            .Select(digits =>
                                {
                                    if (string.IsNullOrEmpty(digits))
                                    {
                                        return null;
                                    }

                                    if (digits.StartsWith('8') || digits.StartsWith('9'))
                                    {
                                        digits = $"0{digits}";
                                    }

                                    return new[]
                                        {
                                            TimeSpan.ParseExact(digits.Substring(0, 4), "%hhmm", null),
                                            TimeSpan.ParseExact(digits.Substring(4, 4), "%hhmm", null),
                                        };
                                })
                            .ToArray();

                        return new ScheduleParsingModel
                        {
                            SpecialtyName = specialtyName,
                            DoctorName = doctorName,
                            ScheduleByDayOfWeek = new Dictionary<DayOfWeek, TimeSpan[]>
                                {
                                    [DayOfWeek.Monday] = schedule[0],
                                    [DayOfWeek.Tuesday] = schedule[1],
                                    [DayOfWeek.Wednesday] = schedule[2],
                                    [DayOfWeek.Thursday] = schedule[3],
                                    [DayOfWeek.Friday] = schedule[4],
                                    [DayOfWeek.Saturday] = schedule[5],
                                }   
                        };
                    })
                .ToArray();
        }

        private static Service[] ParseServices(WebClient webClient, HtmlParser htmlParser)
        {
            Console.WriteLine("Downloading price-list page...");
            var priceListPage = webClient.DownloadString("http://www.ks-klinika.ru/prices");

            Console.WriteLine("Parsing price-list page...");
            var priceListDocument = htmlParser.ParseDocument(priceListPage);

            var priceListTable = priceListDocument
                .QuerySelectorAll("td")
                .Last(x => x.TextContent.TrimStart().StartsWith("Цены на услуги"));

            Specialty lastSpecialty = null;
            var services = new List<Service>();
            foreach (var row in priceListTable.QuerySelectorAll("tr"))
            {
                if (row.Attributes?["bgcolor"] != null)
                {
                    lastSpecialty = new Specialty
                                        {
                                            Name = row.TextContent.Trim(),
                                            Schedules = new List<Schedule>()
                                        };
                    continue;
                }

                var values = row.QuerySelectorAll("td").Select(x => x.TextContent.Trim()).ToArray();

                services.Add(new Service
                               {
                                   Specialty = lastSpecialty,
                                   Description = values[0],
                                   Price = float.Parse(values[1].Replace(" ", string.Empty), NumberStyles.Any, CultureInfo.InvariantCulture),
                                   AdditionalInfo = values[2]
                               });
            }

            return services.ToArray();
        }

        private static Doctor[] ParseDoctors(WebClient webClient, HtmlParser htmlParser)
        {
            Console.WriteLine("Downloading doctors page...");
            var doctorsPage = webClient.DownloadString("http://www.ks-klinika.ru/doctors");

            Console.WriteLine("Parsing doctors page...");
            var doctorsDocument = htmlParser.ParseDocument(doctorsPage);

            var doctorTable = doctorsDocument
                .QuerySelectorAll("td")
                .Last(x => x.TextContent.TrimStart().StartsWith("Врачи КС-Клиники"));

            return doctorTable
                .QuerySelectorAll("tr")
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 2)
                .Select(x =>
                {
                    var rowTitle = x.First().Value;
                    var main = x.Last().Value;

                    var positions = rowTitle.QuerySelector("a.name").TextContent
                            .Split(',', StringSplitOptions.RemoveEmptyEntries).Select(position => position.Trim())
                            .ToArray();

                    var secondName = rowTitle.QuerySelector("span.surname").TextContent.Trim();
                    var fullName = rowTitle.QuerySelector("span.name").TextContent.Trim().Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
                    var imageUrl = (main.QuerySelector("img") as IHtmlImageElement)?.Source;
                    var info = main.QuerySelector(".main").TextContent.Trim();

                    return new Doctor
                    {
                        Positions = positions,
                        FirstName = fullName[0],
                        SecondName = secondName,
                        ThirdName = fullName[1],
                        ImageUrl = imageUrl,
                        Info = info,
                        Schedules = new List<Schedule>(),
                    };
                })
                .ToArray();
        }

        private static void DownloadDoctorImages(WebClient webClient, Doctor[] doctors, string imagesFolder)
        {
            foreach (var doctor in doctors)
            {
                if (string.IsNullOrEmpty(doctor.ImageUrl))
                {
                    continue;
                }

                var imageExtension = Path.GetExtension(doctor.ImageUrl);
                var imageId = $"{doctor.SecondName}{doctor.FirstName.First()}{doctor.ThirdName.First()}{imageExtension}";
                try
                {
                    var image = webClient.DownloadData(doctor.ImageUrl);
                    var path = Path.Combine(imagesFolder, "doctors", imageId);
                    if (!Directory.Exists(Path.Combine(imagesFolder, "doctors")))
                    {
                        Directory.CreateDirectory(Path.Combine(imagesFolder, "doctors"));
                    }

                    File.WriteAllBytes(path, image);
                    doctor.ImageUrl = $"images/doctors/{imageId}";
                }
                catch (Exception exc)
                {
                    Console.WriteLine($"Exception when image downloading: {exc.GetType()}");
                    doctor.ImageUrl = null;
                }
            }
        }

        private class ScheduleParsingModel
        {
            public string SpecialtyName { get; set; }

            public string[] DoctorName { get; set; }

            public Dictionary<DayOfWeek, TimeSpan[]> ScheduleByDayOfWeek { get; set; }
        }
    }
}
