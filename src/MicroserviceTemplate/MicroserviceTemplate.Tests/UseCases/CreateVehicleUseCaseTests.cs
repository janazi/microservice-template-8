using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
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
    public Mock<IValidator<CreateVehicleCommand>> ValidatorMock { get; set; }

    [TestInitialize]
    public void Setup()
    {
        ValidatorMock = new Mock<IValidator<CreateVehicleCommand>>();
    }

    [TestMethod]
    public async Task WhenCommandIsInvalidShouldReturnError()
    {
        // Arrange
        ValidatorMock.Setup(x => x.ValidateAsync(
                It.IsAny<CreateVehicleCommand>(),
                default))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
            {
                new("Vin", "Vin is required")
            }))
            .Verifiable();

        var useCreateVehicleUseCase = new CreateVehicleUseCase(ValidatorMock.Object,
            new Mock<IVehicleRepository>().Object,
            new Mock<IMapper>().Object);

        // Act
        var result = await useCreateVehicleUseCase.ExecuteAsync(new CreateVehicleCommand(""));

        // Assert
        Assert.IsTrue(result.IsFail);
        result.IfFail(c => Assert.IsTrue(c.Message.Contains("Vin is required")));


        ValidatorMock.Verify(x => x.ValidateAsync(
            It.IsAny<CreateVehicleCommand>(),
            default), Times.Once);
    }

    [TestMethod]
    public async Task WhenCommandIsValidShouldCreateVehicle()
    {
        const string vin = "1M8GDM9AXKP042788";
        // Arrange
        ValidatorMock.Setup(x => x.ValidateAsync(
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


        var useCreateVehicleUseCase = new CreateVehicleUseCase(ValidatorMock.Object,
            vehicleRepositoryMock.Object,
            mapperMock.Object);

        // Act
        var result = await useCreateVehicleUseCase.ExecuteAsync(new CreateVehicleCommand(vin));

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.IsNotNull(result.Value);
        Assert.AreEqual(vin, result.Value.Vin);

        ValidatorMock.Verify(x => x.ValidateAsync(
            It.IsAny<CreateVehicleCommand>(),
            default), Times.Once);

        vehicleRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Vehicle>(),
            default), Times.Once);

        mapperMock.Verify(x => x.Map<VehicleViewModel>(
            It.IsAny<Vehicle>()), Times.Once);

    }
}