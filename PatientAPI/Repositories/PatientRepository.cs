using Microsoft.EntityFrameworkCore;
using PatientAPI.Data;
using PatientAPI.Models;

namespace PatientAPI.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Zwraca pacjenta z pełnymi danymi:
    ///   • choroby  
    ///   • recepty + leki  
    ///   • lekarzy (przez recepty)
    /// </summary>
    public async Task<Patient?> GetPatientWithDetailsAsync(int id)
    {
        return await _context.Patients
            .Include(p => p.Illnesses)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Medicaments)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == id);
    }

    /// <summary>
    /// Dodaje nowego pacjenta i od razu zapisuje zmiany.
    /// Używane m.in. przez PrescriptionService, gdy recepta
    /// wystawiana jest dla nieistniejącego jeszcze pacjenta.
    /// </summary>
    public async Task AddPatientAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
    }
}