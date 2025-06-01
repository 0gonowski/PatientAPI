using PatientAPI.DTOs;
using PatientAPI.Models;
using PatientAPI.Repositories;

namespace PatientAPI.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }

    public async Task<PatientGetDto> GetPatientDetailsAsync(int id)
    {
        var patient = await _patientRepository.GetPatientWithDetailsAsync(id);
        if (patient == null)
        {
            return null;
        }

        return new PatientGetDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            Illnesses = patient.Illnesses?.Select(i => new IllnessDto
            {
                IdIllness = i.IdIllness,
                Description = i.Description
            }).ToList(),
            Prescriptions = patient.Prescriptions?
                .OrderBy(p => p.DueDate)
                .Select(p => new PrescriptionDto
                {
                    IdPrescription = p.IdPrescription,
                    Date = p.Date.ToString("yyyy-MM-dd"),
                    DueDate = p.DueDate.ToString("yyyy-MM-dd"),
                    Medicaments = p.Medicaments?.Select(m => new MedicamentDto
                    {
                        IdMedicament = m.IdMedicament,
                        Name = m.Name,
                        Description = m.Description,
                        Dose = m.Dose
                    }).ToList(),
                    Doctor = new DoctorDto
                    {
                        IdDoctor = p.Doctor.IdDoctor,
                        FirstName = p.Doctor.FirstName
                    }
                }).ToList()
        };
    }
}