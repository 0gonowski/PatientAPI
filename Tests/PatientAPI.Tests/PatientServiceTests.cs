using PatientAPI.Models;
using PatientAPI.Repositories;
using PatientAPI.Services;
using Moq;
using Xunit;

namespace PatientAPI.Tests;

public class PatientServiceTests
{
    private readonly Mock<IPatientRepository> _patientRepositoryMock;
    private readonly PatientService _patientService;

    public PatientServiceTests()
    {
        _patientRepositoryMock = new Mock<IPatientRepository>();
        _patientService = new PatientService(_patientRepositoryMock.Object);
    }

    [Fact]
    public async Task GetPatientDetailsAsync_PatientExists_ReturnsPatientDto()
    {
        // Arrange
        var patientId = 1;
        var patient = new Patient
        {
            IdPatient = patientId,
            FirstName = "Jan",
            Illnesses = new List<Illness>
            {
                new Illness { IdIllness = 1, Description = "Flu" }
            },
            Prescriptions = new List<Prescription>
            {
                new Prescription
                {
                    IdPrescription = 1,
                    Date = DateTime.Parse("2012-01-01"),
                    DueDate = DateTime.Parse("2012-01-02"),
                    Medicaments = new List<Medicament>
                    {
                        new Medicament { IdMedicament = 1, Name = "AAA", Description = "Some desc", Dose = 3 }
                    },
                    Doctor = new Doctor { IdDoctor = 1, FirstName = "Dr. Smith" }
                },
                new Prescription
                {
                    IdPrescription = 2,
                    Date = DateTime.Parse("2012-01-01"),
                    DueDate = DateTime.Parse("2012-01-01"),
                    Medicaments = new List<Medicament>
                    {
                        new Medicament { IdMedicament = 2, Name = "BBB", Description = "Another desc", Dose = 2 }
                    },
                    Doctor = new Doctor { IdDoctor = 2, FirstName = "Dr. Jones" }
                }
            }
        };

        _patientRepositoryMock
            .Setup(repo => repo.GetPatientWithDetailsAsync(patientId))
            .ReturnsAsync(patient);

        // Act
        var result = await _patientService.GetPatientDetailsAsync(patientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(patientId, result.IdPatient);
        Assert.Equal("Jan", result.FirstName);
        Assert.Single(result.Illnesses);
        Assert.Equal(2, result.Prescriptions.Count);
        Assert.Equal("2012-01-01", result.Prescriptions[0].DueDate); // Sorted by DueDate
        Assert.Equal("2012-01-02", result.Prescriptions[1].DueDate);
        Assert.Single(result.Prescriptions[0].Medicaments);
        Assert.Equal("Dr. Smith", result.Prescriptions[1].Doctor.FirstName);
    }

    [Fact]
    public async Task GetPatientDetailsAsync_PatientNotFound_ReturnsNull()
    {
        // Arrange
        _patientRepositoryMock
            .Setup(repo => repo.GetPatientWithDetailsAsync(It.IsAny<int>()))
            .ReturnsAsync((Patient)null);

        // Act
        var result = await _patientService.GetPatientDetailsAsync(999);

        // Assert
        Assert.Null(result);
    }
}