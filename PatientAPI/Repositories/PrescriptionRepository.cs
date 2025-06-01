using Microsoft.EntityFrameworkCore;
using PatientAPI.Data;
using PatientAPI.Models;

namespace PatientAPI.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly AppDbContext _context;

    public PrescriptionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddPrescriptionAsync(Prescription prescription)
    {
        await _context.Prescriptions.AddAsync(prescription);
        await _context.SaveChangesAsync();
    }

    public Task<bool> MedicamentsExistAsync(IEnumerable<int> ids)
        => _context.Medicaments.CountAsync(m => ids.Contains(m.IdMedicament))
            .ContinueWith(t => t.Result == ids.Count());
}