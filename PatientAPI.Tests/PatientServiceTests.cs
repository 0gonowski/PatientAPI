using PatientAPI.DTOs;
using PatientAPI.Models;
using PatientAPI.Repositories;

namespace PatientAPI.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repo;

    public PatientService(IPatientRepository repo) => _repo = repo;

    // 1️⃣  GET z detalami (mapujemy do DTO)
    public async Task<PatientGetDto?> GetPatientDetailsAsync(int id)
    {
        var patient = await _repo.GetPatientWithDetailsAsync(id);
        if (patient is null) return null;

        return new PatientGetDto
        {
            IdPatient   = patient.IdPatient,
            FirstName   = patient.FirstName,
            Illnesses   = patient.Illnesses
                            .Select(i => new IllnessDto { IdIllness = i.IdIllness, Description = i.Description })
                            .ToList(),
            // sortujemy recepty rosnąco po DueDate
            Prescriptions = patient.Prescriptions
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDto
                {
                    IdPrescription = p.IdPrescription,
                    Date           = p.Date.ToString("yyyy-MM-dd"),
                    DueDate        = p.DueDate.ToString("yyyy-MM-dd"),
                    Medicaments    = p.Medicaments
                                        .Select(m => new MedicamentDto
                                        {
                                            IdMedicament = m.IdMedicament,
                                            Name         = m.Name,
                                            Description  = m.Description,
                                            Dose         = m.Dose
                                        }).ToList(),
                    Doctor = new DoctorDto
                    {
                        IdDoctor  = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName
                    }
                })
                .ToList()
        };
    }

    // 2️⃣  CREATE (zwraca świeżo utworzony DTO)
    public async Task<PatientGetDto> CreatePatientAsync(PatientGetDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName))
            throw new ArgumentException("FirstName is required", nameof(dto));

        var newPatient = new Patient
        {
            FirstName = dto.FirstName,
            Illnesses = new List<Illness>(),
            Prescriptions = new List<Prescription>()
        };

        await _repo.AddPatientAsync(newPatient);

        // po Callback-u w teście newPatient.IdPatient zostaje ustawione
        return new PatientGetDto
        {
            IdPatient = newPatient.IdPatient,
            FirstName = newPatient.FirstName,
            Illnesses = new(),          // pusto – dopiero co stworzone
            Prescriptions = new()
        };
    }
}
