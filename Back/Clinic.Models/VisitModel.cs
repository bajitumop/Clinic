namespace Clinic.Models
{
    using System;

    public class VisitModel
    {
        public long Id { get; set; }
        public long DoctorId { get; set; }
        public long ServiceId { get; set; }
        public DateTime DateTime { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorSecondName { get; set; }
        public string DoctorThirdName { get; set; }
        public string ServiceDescription { get; set; }
        public string Specialty { get; set; }
    }
}
