namespace Clinic.Models.Doctors
{
    public class DoctorModel
    {
        public long Id { get; set; }

        public string Specialty { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string ThirdName { get; set; }

        public string Info { get; set; }

        public string ImageUrl => $"/api/images/doctors/{Id}";
    }
}
