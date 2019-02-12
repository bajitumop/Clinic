﻿namespace Clinic.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Speciality
    {
        [Key]
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
