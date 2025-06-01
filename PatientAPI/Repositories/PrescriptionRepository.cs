using Microsoft.EntityFrameworkCore;
using PatientAPI.Data;
using PatientAPI.Models;

namespace PatientAPI.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private readonly AppDbContext _ctx;
    public PrescriptionRepository(AppDbContext ctx) => _ctx = ctx;

    public async Task AddPrescriptionAsync(Prescription prescription)
    {
        await _ctx.Prescriptions.AddAsync(prescription);
        await _ctx.SaveChangesAsync();
    }

    public async Task<bool> MedicamentsExistAsync(IEnumerable<int> ids)
    {
        // policz ile leków z listy faktycznie istnieje
        var existing = await _ctx.Medicaments
            .CountAsync(m => ids.Contains(m.IdMedicament));
        return existing == ids.Count();
    }
}