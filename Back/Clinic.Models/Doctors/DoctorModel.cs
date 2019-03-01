namespace Clinic.Models.Doctors
{
    public class DoctorModel : DoctorShortModel
    {
        public string[] Positions { get; set; }

        public string Info { get; set; }

        public int Room { get; set; }
    }
}
