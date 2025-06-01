namespace PatientAPI.Models;

public class Patient
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; }
    public List<Illness> Illnesses { get; set; }
    public List<Prescription> Prescriptions { get; set; }
}