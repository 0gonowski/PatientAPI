using PatientAPI.Models;

namespace PatientAPI.Repositories;

public interface IPrescriptionRepository
{
    Task AddPrescriptionAsync(Prescription prescription);
    Task<bool> MedicamentsExistAsync(IEnumerable<int> ids);
}