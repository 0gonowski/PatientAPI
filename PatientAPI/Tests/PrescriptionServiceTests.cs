using Moq;
using PatientAPI.DTOs;
using PatientAPI.Models;
using PatientAPI.Repositories;
using PatientAPI.Services;
using Xunit;

namespace PatientAPI.Tests;

public class PrescriptionServiceTests
{
    private readonly PrescriptionService _service;
    private readonly Mock<IPatientRepository> _patientRepoMock = new();
    private readonly Mock<IPrescriptionRepository> _prescriptionRepoMock = new();

    public PrescriptionServiceTests()
    {
        _service = new PrescriptionService(_patientRepoMock.Object, _prescriptionRepoMock.Object);
    }

    [Fact]
    public async Task CreatePrescriptionAsync_TooManyMedicaments_ThrowsArgumentException()
    {
        var dto = new PrescriptionCreateDto
        {
            Date = DateTime.Today,
            DueDate = DateTime.Today.AddDays(1),
            IdPatient = 1,
            DoctorId = 1,
            MedicamentIds = Enumerable.Range(1, 11).ToList()
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePrescriptionAsync(dto));
    }
}