namespace Clinic.Initializer.Migrations.Sql
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
    ""Specialty"" varchar not null,
    ""FirstName"" varchar not null,
    ""SecondName"" varchar not null,
    ""ThirdName"" varchar not null,
    ""Info"" text
);

create table images(
    ""Id"" serial primary key,
    ""Format"" varchar not null,
    ""Content"" bytea not null
);

create table schedules(
    ""DoctorId"" serial primary key,
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
    ""SaturdayEnd"" interval
);

create table services(
    ""Id"" serial primary key,
    ""Specialty"" varchar not null,
    ""Description"" varchar not null,
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
);";
    }
}
