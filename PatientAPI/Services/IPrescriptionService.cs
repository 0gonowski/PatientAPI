using PatientAPI.DTOs;

namespace PatientAPI.Services;

public interface IPrescriptionService
{
    Task<PrescriptionDto> CreatePrescriptionAsync(PrescriptionCreateDto dto);
}