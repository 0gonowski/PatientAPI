using System.Collections.Generic;

namespace PatientAPI.DTOs;

public class PatientGetDto
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public List<IllnessDto> Illnesses { get; set; }
    public List<PrescriptionDto> Prescriptions { get; set; }
}

public class IllnessDto
{
    public int IdIllness { get; set; }
    public string Description { get; set; }
}

public class PrescriptionDto
{
    public int IdPrescription { get; set; }
    public string Date { get; set; }
    public string DueDate { get; set; }
    public List<MedicamentDto> Medicaments { get; set; }
    public DoctorDto Doctor { get; set; }
}

public class MedicamentDto
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Dose { get; set; }
}

public class DoctorDto
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
}