using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using PatientAPI.DTOs;
using PatientAPI.Models;
using PatientAPI.Repositories;
using PatientAPI.Services;
using Xunit;

namespace PatientAPI.Tests;

public class PrescriptionServiceTests
{
    private readonly Mock<IPrescriptionRepository> _prescRepoMock = new();
    private readonly Mock<IPatientRepository>      _patientRepoMock = new();
    private readonly PrescriptionService           _service;

    public PrescriptionServiceTests()
    {
        _service = new PrescriptionService(_prescRepoMock.Object, _patientRepoMock.Object);
    }

    [Fact]
    public async Task CreatePrescriptionAsync_ValidInput_CreatesPrescription()
    {
        // arrange
        var dto = new PrescriptionCreateDto
        {
            IdPatient    = 1,
            DoctorId      = 1,
            Date          = DateTime.Parse("2024-01-01"),
            DueDate       = DateTime.Parse("2024-01-10"),
            MedicamentIds = new List<int> { 1 }
        };

        _patientRepoMock
            .Setup(r => r.GetPatientWithDetailsAsync(dto.IdPatient))
            .ReturnsAsync(new Patient { IdPatient = 1, FirstName = "Jan" });

        _prescRepoMock
            .Setup(r => r.MedicamentsExistAsync(dto.MedicamentIds))
            .ReturnsAsync(true);

        _prescRepoMock
            .Setup(r => r.AddPrescriptionAsync(It.IsAny<Prescription>()))
            .Callback<Prescription>(p => p.IdPrescription = 5)
            .Returns(Task.CompletedTask);

        // act
        var result = await _service.CreatePrescriptionAsync(dto);

        // assert
        Assert.Equal(5, result.IdPrescription);
        Assert.Equal("2024-01-10", result.DueDate);
        _prescRepoMock.Verify(r => r.AddPrescriptionAsync(It.IsAny<Prescription>()), Times.Once);
    }

    [Fact]
    public async Task CreatePrescriptionAsync_TooManyMedicaments_Throws()
    {
        var dto = new PrescriptionCreateDto
        {
            IdPatient     = 1,
            DoctorId      = 1,
            Date          = DateTime.Parse("2024-01-01"),
            DueDate       = DateTime.Parse("2024-01-02"),
            MedicamentIds = new List<int> { 1,2,3,4,5,6,7,8,9,10,11 }
        };

        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePrescriptionAsync(dto));
    }
}
