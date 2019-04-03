namespace Clinic.Domain
{
    using System;

    [Flags]
    public enum DoctorPermission : long
    {
        None = 0,
        All = long.MaxValue,
    }
}
