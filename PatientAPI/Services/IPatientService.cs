using PatientAPI.DTOs;

namespace PatientAPI.Services;

public interface IPatientService
{
    Task<PatientGetDto> GetPatientDetailsAsync(int id);
}