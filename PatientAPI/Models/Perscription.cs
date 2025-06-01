using System;
using System.Collections.Generic;

namespace PatientAPI.Models;

public class Prescription
{
    public int IdPrescription { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdPatient { get; set; }
    public Patient Patient { get; set; }
    public List<Medicament> Medicaments { get; set; }
    public int DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    
    public byte[] RowVersion { get; set; }
}