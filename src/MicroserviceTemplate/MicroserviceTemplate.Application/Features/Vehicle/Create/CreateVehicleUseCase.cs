using AutoMapper;
using FluentValidation;
using LanguageExt.Common;
using MicroserviceTemplate.Application.Features.Vehicle.UseCases;
using MicroserviceTemplate.Domain.Repositories;

namespace MicroserviceTemplate.Application.Features.Vehicle.Create;

public class CreateVehicleUseCase(IValidator<CreateVehicleCommand> validator,
    IVehicleRepository vehicleRepository,
    IMapper mapper) : ICreateVehicleUseCase
{
    public async Task<Result<VehicleViewModel>> ExecuteAsync(CreateVehicleCommand command,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
        {
            var error = new ValidationException(validationResult.Errors);
            return new Result<VehicleViewModel>(error);
        }

        var vehicle = new Domain.Entities.Vehicle(command.Vin);
        var operationResult = await vehicleRepository.CreateAsync(vehicle, cancellationToken);
        if (!operationResult.IsSuccess) return new Result<VehicleViewModel>(operationResult.Error);

        var vehicleViewModel = mapper.Map<VehicleViewModel>(vehicle);
        return new Result<VehicleViewModel>(vehicleViewModel);
    }
}