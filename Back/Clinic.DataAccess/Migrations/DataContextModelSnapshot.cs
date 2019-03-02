﻿// <auto-generated />
using System;
using Clinic.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Clinic.DataAccess.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Clinic.Domain.Doctor", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<long?>("ImageId");

                    b.Property<string>("Info");

                    b.Property<string[]>("Positions");

                    b.Property<int>("Room");

                    b.Property<string>("SecondName")
                        .IsRequired();

                    b.Property<string>("ThirdName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("Clinic.Domain.Image", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Content")
                        .IsRequired();

                    b.Property<string>("Format")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Clinic.Domain.Schedule", b =>
                {
                    b.Property<long>("DoctorId");

                    b.Property<long>("SpecialtyId");

                    b.Property<TimeSpan?>("FridayEnd");

                    b.Property<TimeSpan?>("FridayStart");

                    b.Property<TimeSpan?>("MondayEnd");

                    b.Property<TimeSpan?>("MondayStart");

                    b.Property<TimeSpan?>("SaturdayEnd");

                    b.Property<TimeSpan?>("SaturdayStart");

                    b.Property<TimeSpan?>("ThursdayEnd");

                    b.Property<TimeSpan?>("ThursdayStart");

                    b.Property<TimeSpan?>("TuesdayEnd");

                    b.Property<TimeSpan?>("TuesdayStart");

                    b.Property<TimeSpan>("VisitDuration");

                    b.Property<TimeSpan?>("WednesdayEnd");

                    b.Property<TimeSpan?>("WednesdayStart");

                    b.Property<string>("Weekdays");

                    b.HasKey("DoctorId", "SpecialtyId");

                    b.HasIndex("SpecialtyId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("Clinic.Domain.Service", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalInfo");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<float>("Price");

                    b.Property<long>("SpecialtyId");

                    b.HasKey("Id");

                    b.HasIndex("SpecialtyId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Clinic.Domain.Specialty", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Specialties");
                });

            modelBuilder.Entity("Clinic.Domain.User", b =>
                {
                    b.Property<string>("Username")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("PasswordHash")
                        .IsRequired();

                    b.Property<int>("Permission");

                    b.Property<long>("Phone");

                    b.Property<string>("SecondName")
                        .IsRequired();

                    b.Property<string>("ThirdName");

                    b.HasKey("Username");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Clinic.Domain.Visit", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTime");

                    b.Property<long>("DoctorId");

                    b.Property<long>("ServiceId");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("ServiceId");

                    b.ToTable("Visits");
                });

            modelBuilder.Entity("Clinic.Domain.Doctor", b =>
                {
                    b.HasOne("Clinic.Domain.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("Clinic.Domain.Schedule", b =>
                {
                    b.HasOne("Clinic.Domain.Doctor", "Doctor")
                        .WithMany("Schedules")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Clinic.Domain.Specialty", "Specialty")
                        .WithMany("Schedules")
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Clinic.Domain.Service", b =>
                {
                    b.HasOne("Clinic.Domain.Specialty", "Specialty")
                        .WithMany("Services")
                        .HasForeignKey("SpecialtyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Clinic.Domain.Visit", b =>
                {
                    b.HasOne("Clinic.Domain.Doctor", "Doctor")
                        .WithMany("Visits")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Clinic.Domain.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
