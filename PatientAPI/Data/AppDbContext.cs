using Microsoft.EntityFrameworkCore;
using System;

namespace PatientAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Illness> Illnesses { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---- SEED DATA ----------------------------------------------------
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { IdDoctor = 1, FirstName = "AAA" },
            new Doctor { IdDoctor = 2, FirstName = "BBB" }
        );

        modelBuilder.Entity<Patient>().HasData(
            new Patient { IdPatient = 1, FirstName = "Jan" },
            new Patient { IdPatient = 2, FirstName = "Anna" }
        );

        modelBuilder.Entity<Illness>().HasData(
            new Illness { IdIllness = 1, Description = "Flu" },
            new Illness { IdIllness = 2, Description = "Cold" }
        );

        modelBuilder.Entity<Medicament>().HasData(
            new Medicament { IdMedicament = 1, Name = "AAA", Description = "Some desc", Dose = 1 },
            new Medicament { IdMedicament = 2, Name = "BBB", Description = "Other desc", Dose = 2 }
        );

        modelBuilder.Entity<Prescription>().HasData(
            new Prescription
            {
                IdPrescription = 1,
                Date = DateTime.Parse("2012-01-01"),
                DueDate = DateTime.Parse("2012-01-10"),
                IdPatient = 1,
                DoctorId = 1
            },
            new Prescription
            {
                IdPrescription = 2,
                Date = DateTime.Parse("2013-03-01"),
                DueDate = DateTime.Parse("2013-03-05"),
                IdPatient = 2,
                DoctorId = 2
            }
        );

        // many-to-many between Prescriptions and Medicaments
        modelBuilder.Entity<Prescription>()
            .HasMany(p => p.Medicaments)
            .WithMany()
            .UsingEntity(j =>
            {
                j.HasData(
                    new { PrescriptionsIdPrescription = 1, MedicamentsIdMedicament = 1 },
                    new { PrescriptionsIdPrescription = 1, MedicamentsIdMedicament = 2 },
                    new { PrescriptionsIdPrescription = 2, MedicamentsIdMedicament = 2 }
                );
            });

        // ---- CONFIGURATIONS ----------------------------------------------
        modelBuilder.Entity<Prescription>()
            .Property(p => p.RowVersion)
            .IsRowVersion();
    }
}