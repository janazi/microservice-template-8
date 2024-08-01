using MicroserviceTemplate.Domain.Entities;

namespace MicroserviceTemplate.Application.Features.Vehicle.Create;

public interface ICreateVehicleUseCase
{
    Task<Result<VehicleViewModel>> ExecuteAsync(CreateVehicleCommand command, CancellationToken cancellationToken = default);
}