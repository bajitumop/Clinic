using System;
using Clinic.Domain;

namespace Clinic.Models.Schedules
{
    public class VisitStatusInfoModel
    {
        public DateTime DateTime { get; set; }

        public VisitStatus Status { get; set; }
    }
}
