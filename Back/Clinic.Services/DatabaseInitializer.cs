namespace Clinic.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    using AngleSharp.Html.Dom;
    using AngleSharp.Html.Parser;
    using DataAccess.Repositories;
    using Domain;

    using Microsoft.Extensions.Logging;

    public class DatabaseInitializer
    {
        private readonly ILogger logger;
        private readonly IDoctorsRepository doctorsRepository;
        private readonly IServicesRepository serviceRepository;
        private readonly IImagesRepository imagesRepository;
        private readonly ISchedulesRepository schedulesRepository;
        private readonly IUsersRepository usersRepository;
        private readonly CryptoService cryptoService;

        public DatabaseInitializer(
            IDoctorsRepository doctorsRepository, 
            IImagesRepository imagesRepository, 
            IServicesRepository serviceRepository, 
            ISchedulesRepository schedulesRepository,
            IUsersRepository usersRepository,
            CryptoService cryptoService,
            ILogger<DatabaseInitializer> logger)
        {
            this.logger = logger;
            this.schedulesRepository = schedulesRepository;
            this.usersRepository = usersRepository;
            this.cryptoService = cryptoService;
            this.serviceRepository = serviceRepository;
            this.doctorsRepository = doctorsRepository;
            this.imagesRepository = imagesRepository;
        }

        public async Task Init()
        {
            var htmlParser = new HtmlParser();
            using (var webClient = new WebClient { Encoding = Encoding.GetEncoding("Windows-1251") })
            {
                var doctorParsingModels = await this.ParseDoctors(webClient, htmlParser);
                var services = await this.ParseServices(webClient, htmlParser);
                var specialties = services.Select(x => x.Specialty).Distinct().ToArray();
                var scheduleParsingModels = await this.ParseScheduleParsingModels(webClient, htmlParser);
                await this.DownloadDoctorImages(webClient, doctorParsingModels);

                foreach (var scheduleParsingModel in scheduleParsingModels)
                {
                    var specialty = specialties.FirstOrDefault(x => x == scheduleParsingModel.SpecialtyName);

                    var doctorParsingModel = doctorParsingModels.FirstOrDefault(
                        d => new[] { d.FirstName, d.SecondName, d.ThirdName }.Intersect(scheduleParsingModel.DoctorName).Count()
                             == scheduleParsingModel.DoctorName.Length);

                    if (doctorParsingModel == null)
                    {
                        this.logger.LogWarning("Doctor not found by schedule!");
                        continue;
                    }

                    if (specialty == null)
                    {
                        this.logger.LogWarning("Specialty not found by schedule!");
                        continue;
                    }

                    var doctor = new Doctor
                    {
                        FirstName = doctorParsingModel.FirstName,
                        SecondName = doctorParsingModel.SecondName,
                        ThirdName = doctorParsingModel.ThirdName,
                        Info = doctorParsingModel.Info,
                        Positions = doctorParsingModel.Positions
                    };

                    await this.doctorsRepository.CreateAsync(doctor);
                
                    if (doctorParsingModel.Image != null)
                    {
                        await this.imagesRepository.UpsertAsync(doctor.Id, doctorParsingModel.Image, doctorParsingModel.ImageFormat);
                    }

                    await this.AddWeekdays(doctor);

                    var schedule = new Schedule
                    {
                        DoctorId = doctor.Id,
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
                        SaturdayEnd = scheduleParsingModel.ScheduleByDayOfWeek[DayOfWeek.Saturday]?[1],
                        VisitDuration = TimeSpan.FromMinutes(10)
                    };

                    await this.schedulesRepository.UpsertAsync(schedule);
                }

                foreach (var service in services)
                {
                    await this.serviceRepository.CreateAsync(service);
                }

                var admin = new User
                {
                    Username = "admin",
                    FirstName = "Администратор",
                    SecondName = "Администратор",
                    ThirdName = "Администратор",
                    UserPermission = UserPermission.All,
                    Phone = 89999999999,
                    PasswordHash = this.cryptoService.Encrypt("admin")
                };
                
                await this.usersRepository.CreateAsync(admin);



                this.logger.LogInformation("Import data was successfully finished");
            }
        }

        private async Task AddWeekdays(Doctor doctor)
        {
            // ToDo: Implement weekdays initializing 
            this.logger.LogWarning("Implement weekdays initializing!");
        }

        private async Task<ScheduleParsingModel[]> ParseScheduleParsingModels(WebClient webClient, HtmlParser htmlParser)
        {
            this.logger.LogInformation("Downloading schedule page...");
            var schedulePage = await webClient.DownloadStringTaskAsync("http://www.ks-klinika.ru/raspisanie");

            this.logger.LogInformation("Parsing schedule page...");
            var scheduleDocument = await htmlParser.ParseDocumentAsync(schedulePage);

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

        private async Task<Service[]> ParseServices(WebClient webClient, HtmlParser htmlParser)
        {
            this.logger.LogInformation("Downloading price-list page...");
            var priceListPage = await webClient.DownloadStringTaskAsync("http://www.ks-klinika.ru/prices");

            this.logger.LogInformation("Parsing price-list page...");
            var priceListDocument = await htmlParser.ParseDocumentAsync(priceListPage);

            var priceListTable = priceListDocument
                .QuerySelectorAll("td")
                .Last(x => x.TextContent.TrimStart().StartsWith("Цены на услуги"));

            var lastSpecialty = string.Empty;
            var services = new List<Service>();
            foreach (var row in priceListTable.QuerySelectorAll("tr"))
            {
                if (row.Attributes?["bgcolor"] != null)
                {
                    lastSpecialty = row.TextContent.Trim();
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

        private async Task<DoctorParsingModel[]> ParseDoctors(WebClient webClient, HtmlParser htmlParser)
        {
            this.logger.LogInformation("Downloading doctors page...");
            var doctorsPage = await webClient.DownloadStringTaskAsync("http://www.ks-klinika.ru/doctors");

            this.logger.LogInformation("Parsing doctors page...");
            var doctorsDocument = await htmlParser.ParseDocumentAsync(doctorsPage);

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

                    return new DoctorParsingModel
                    {
                        Positions = positions,
                        FirstName = fullName[0],
                        SecondName = secondName,
                        ThirdName = fullName[1],
                        ImageUrl = imageUrl,
                        Info = info
                    };
                })
                .ToArray();
        }

        private async Task DownloadDoctorImages(WebClient webClient, DoctorParsingModel[] doctors)
        {
            foreach (var doctor in doctors)
            {
                if (!string.IsNullOrEmpty(doctor.ImageUrl))
                {
                    doctor.ImageFormat = "image/jpeg";
                    doctor.Image = await webClient.DownloadDataTaskAsync(doctor.ImageUrl);
                }
            }
        }

        private class DoctorParsingModel
        {
            public string[] Positions { get; set; }

            public string FirstName { get; set; }

            public string SecondName { get; set; }

            public string ThirdName { get; set; }

            public string ImageUrl { get; set; }

            public string Info { get; set; }

            public byte[] Image { get; set; }

            public string ImageFormat { get; set; }
        }

        private class ScheduleParsingModel
        {
            public string SpecialtyName { get; set; }

            public string[] DoctorName { get; set; }

            public Dictionary<DayOfWeek, TimeSpan[]> ScheduleByDayOfWeek { get; set; }
        }
    }
}
