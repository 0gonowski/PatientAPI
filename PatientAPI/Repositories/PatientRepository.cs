using PatientAPI.Data;
using PatientAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace PatientAPI.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Patient> GetPatientWithDetailsAsync(int id)
    {
        return await _context.Patients
            .Include(p => p.Illnesses)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Medicaments)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .FirstOrDefaultAsync(p => p.IdPatient == id);
    }
}