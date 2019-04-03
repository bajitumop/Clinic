namespace Clinic.Models.Doctors
{
    public class DoctorShortModel
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        public string ImageUrl { get; set; }

        public string[] Specialties { get; set; }
    }
}
