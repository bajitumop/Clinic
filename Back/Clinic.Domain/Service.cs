﻿namespace Clinic.Domain
{
    using System.ComponentModel.DataAnnotations;

    public class Service : Entity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Specialty Specialty { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public float Price { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
