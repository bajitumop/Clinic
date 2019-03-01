﻿namespace Clinic.Models
{
    public class ServiceModel
    {
        public long Id { get; set; }

        public long SpecialtyId { get; set; }

        public string SpecialtyName { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
