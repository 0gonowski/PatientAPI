using System;
using System.Collections.Generic;

namespace PatientAPI.DTOs;

public class PrescriptionCreateDto
{
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdPatient { get; set; }
    public int DoctorId { get; set; }
    public List<int> MedicamentIds { get; set; }
}