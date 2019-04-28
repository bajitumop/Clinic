﻿namespace Clinic.Initializer.Migrations.Sql
{
    public class m20190318230900_InitialCreate : BaseScriptMigration
    {
        protected override string Script => @"
create table users (
    ""Username"" varchar(200) primary key,
    ""PasswordHash"" varchar not null,
    ""FirstName"" varchar not null,
    ""SecondName"" varchar not null,
    ""ThirdName"" varchar,
    ""UserPermission"" int not null
);

create table doctors(
    ""Id"" serial primary key,
    ""FirstName"" varchar not null,
    ""SecondName"" varchar not null,
    ""ThirdName"" varchar not null,
    ""Info"" text,
    ""ImageId"" varchar,
    ""Positions"" text[],
    ""DoctorPermission"" bigint not null
);

create table images(
    ""Id"" serial primary key,
    ""Format"" varchar not null,
    ""Content"" bytea not null
);

create table schedules(
    ""DoctorId"" bigint not null,
    ""Specialty"" varchar not null,
    ""VisitDuration"" interval not null,
    ""MondayStart"" interval,
    ""MondayEnd"" interval,
    ""TuesdayStart"" interval,
    ""TuesdayEnd"" interval,
    ""WednesdayStart"" interval,
    ""WednesdayEnd"" interval,
    ""ThursdayStart"" interval,
    ""ThursdayEnd"" interval,
    ""FridayStart"" interval,
    ""FridayEnd"" interval,
    ""SaturdayStart"" interval,
    ""SaturdayEnd"" interval,
    primary key (""DoctorId"", ""Specialty"")
);

create table services(
    ""Id"" serial primary key,
    ""Specialty"" varchar not null,
    ""Description"" varchar not null,
    ""DoctorPermission"" bigint not null,
    ""Price"" real not null,
    ""AdditionalInfo"" text
);

create table visits(
    ""Id"" serial primary key,
    ""Username"" varchar not null,
    ""DoctorId"" bigint not null,
    ""ServiceId"" bigint not null,
    ""DateTime"" timestamp not null,
    ""VisitStatus"" int not null,
    ""RoomNumber"" varchar not null
);

create table weekdays(""DoctorId"" bigint not null,  ""Date"" timestamp not null);
ALTER TABLE weekdays ADD FOREIGN KEY(""DoctorId"") REFERENCES doctors(""Id"") ON DELETE CASCADE;";
    }
}
