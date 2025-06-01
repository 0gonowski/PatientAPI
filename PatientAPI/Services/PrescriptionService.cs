using PatientAPI.DTOs;
using PatientAPI.Models;
using PatientAPI.Repositories;

namespace PatientAPI.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IPrescriptionRepository _prescriptionRepo;
    private readonly IPatientRepository      _patientRepo;

    public PrescriptionService(IPrescriptionRepository prescriptionRepo,
                               IPatientRepository      patientRepo)
    {
        _prescriptionRepo = prescriptionRepo;
        _patientRepo      = patientRepo;
    }

    public async Task<PrescriptionDto> CreatePrescriptionAsync(PrescriptionCreateDto dto)
    {
        // ── walidacje
        if (dto.MedicamentIds is null || !dto.MedicamentIds.Any())
            throw new ArgumentException("At least one medicament is required.");

        if (dto.MedicamentIds.Count > 10)
            throw new ArgumentException("A prescription cannot contain more than 10 medicaments.");

        if (dto.DueDate < dto.Date)
            throw new ArgumentException("DueDate cannot be earlier than Date.");

        if (!await _prescriptionRepo.MedicamentsExistAsync(dto.MedicamentIds))
            throw new ArgumentException("One or more medicaments do not exist.");

        // ── pacjent (może trzeba utworzyć nowego)
        var patient = await _patientRepo.GetPatientWithDetailsAsync(dto.IdPatient);
        if (patient is null)
        {
            patient = new Patient { FirstName = "Unknown" };
            await _patientRepo.AddPatientAsync(patient);
        }

        // ── encja recepty
        var prescription = new Prescription
        {
            Date        = dto.Date,
            DueDate     = dto.DueDate,
            Patient     = patient,
            DoctorId    = dto.DoctorId,
            Medicaments = dto.MedicamentIds
                           .Select(id => new Medicament { IdMedicament = id })
                           .ToList()
        };

        await _prescriptionRepo.AddPrescriptionAsync(prescription);

        // ── mapowanie DTO
        return new PrescriptionDto
        {
            IdPrescription = prescription.IdPrescription,
            Date           = prescription.Date.ToString("yyyy-MM-dd"),
            DueDate        = prescription.DueDate.ToString("yyyy-MM-dd"),
            Medicaments    = prescription.Medicaments.Select(m => new MedicamentDto
            {
                IdMedicament = m.IdMedicament,
                Name         = m.Name,
                Description  = m.Description,
                Dose         = m.Dose
            }).ToList(),
            Doctor = new DoctorDto { IdDoctor = dto.DoctorId }
        };
    }
}
