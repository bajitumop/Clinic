namespace Clinic.Domain
{
    using System;

    [Flags]
    public enum UserPermission
    {
        CanVisitDoctor = 1,
        All = int.MaxValue
    }
}