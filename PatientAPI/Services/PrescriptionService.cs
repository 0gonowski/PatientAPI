using PatientAPI.DTOs;
using PatientAPI.Exceptions;
using PatientAPI.Models;
using PatientAPI.Repositories;

namespace PatientAPI.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPatientRepository _patientRepo;
    private readonly IPrescriptionRepository _prescriptionRepo;

    public PrescriptionService(IPatientRepository patientRepo, IPrescriptionRepository prescriptionRepo)
    {
        _patientRepo = patientRepo;
        _prescriptionRepo = prescriptionRepo;
    }

    public async Task<PrescriptionDto> CreatePrescriptionAsync(PrescriptionCreateDto dto)
    {
        if (dto.MedicamentIds == null || !dto.MedicamentIds.Any())
            throw new ArgumentException("At least one medicament is required.");
        if (dto.MedicamentIds.Count > 10)
            throw new ArgumentException("A prescription cannot contain more than 10 medicaments.");
        if (dto.DueDate < dto.Date)
            throw new ArgumentException("DueDate cannot be earlier than Date.");
        
        if (!await _prescriptionRepo.MedicamentsExistAsync(dto.MedicamentIds))
            throw new ArgumentException("One or more medicaments do not exist.");
        
        var patient = await _patientRepo.GetPatientWithDetailsAsync(dto.IdPatient);
        if (patient == null)
        {
            patient = new Patient { FirstName = "Unknown" }; // minimalny pacjent
            await _patientRepo.AddPatientAsync(patient);
        }

        var prescription = new Prescription
        {
            Date = dto.Date,
            DueDate = dto.DueDate,
            Patient = patient,
            DoctorId = dto.DoctorId,
            Medicaments = dto.MedicamentIds.Select(id => new Medicament { IdMedicament = id }).ToList()
        };

        await _prescriptionRepo.AddPrescriptionAsync(prescription);
        
        return new PrescriptionDto
        {
            IdPrescription = prescription.IdPrescription,
            Date = prescription.Date.ToString("yyyy-MM-dd"),
            DueDate = prescription.DueDate.ToString("yyyy-MM-dd"),
            Medicaments = prescription.Medicaments.Select(m => new MedicamentDto
            {
                IdMedicament = m.IdMedicament
            }).ToList(),
            Doctor = new DoctorDto { IdDoctor = dto.DoctorId }
        };
    }
}