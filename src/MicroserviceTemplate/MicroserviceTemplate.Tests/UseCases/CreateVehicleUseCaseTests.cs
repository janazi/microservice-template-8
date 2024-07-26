using AutoMapper;
using FluentValidation;
using MicroserviceTemplate.Application.Features.Vehicle;
using MicroserviceTemplate.Application.Features.Vehicle.Create;
using MicroserviceTemplate.Domain.Entities;
using MicroserviceTemplate.Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace MicroserviceTemplate.Tests.UseCases;

[TestClass]
public class CreateVehicleUseCaseTests
{
    [TestMethod]
    public async Task WhenCommandIsValidShouldCreateVehicle()
    {
        const string vin = "1M8GDM9AXKP042788";
        // Arrange
        var validatorMock = new Mock<IValidator<CreateVehicleCommand>>();
        validatorMock.Setup(x => x.ValidateAsync(
                It.IsAny<CreateVehicleCommand>(),
                default))
            .ReturnsAsync(new FluentValidation.Results.ValidationResult())
            .Verifiable();

        var vehicleRepositoryMock = new Mock<IVehicleRepository>();
        vehicleRepositoryMock.Setup(x => x.CreateAsync(
                It.IsAny<Vehicle>(),
                default))
            .ReturnsAsync(new OperationResult<Vehicle>(new Vehicle(vin)))
            .Verifiable();

        var mapperMock = new Mock<IMapper>();
        mapperMock.Setup(x => x.Map<VehicleViewModel>(
                It.IsAny<Vehicle>()))
            .Returns(new VehicleViewModel { Id = Guid.NewGuid().ToString(), Vin = vin })
            .Verifiable();


        var useCreateVehicleUseCase = new CreateVehicleUseCase(validatorMock.Object,
            vehicleRepositoryMock.Object,
            mapperMock.Object);

        // Act
        var result = await useCreateVehicleUseCase.ExecuteAsync(new CreateVehicleCommand(vin));

        // Assert
        Assert.IsTrue(result.IsSuccess);

        var vehicle = result.Match(
            vehicle => vehicle,
            error => throw error);

        Assert.IsNotNull(vehicle);
        Assert.AreEqual(vin, vehicle.Vin);

        validatorMock.Verify(x => x.ValidateAsync(
            It.IsAny<CreateVehicleCommand>(),
            default), Times.Once);

        vehicleRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Vehicle>(),
            default), Times.Once);

        mapperMock.Verify(x => x.Map<VehicleViewModel>(
            It.IsAny<Vehicle>()), Times.Once);

    }
}