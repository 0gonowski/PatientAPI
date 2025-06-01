using PatientAPI.Models;

namespace PatientAPI.Repositories;

public interface IPatientRepository
{
    Task<Patient?> GetPatientWithDetailsAsync(int id);
    
    Task AddPatientAsync(Patient patient);
}