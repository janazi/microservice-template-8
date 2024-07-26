using LanguageExt.Common;
using MicroserviceTemplate.Application.Features.Vehicle.Create;

namespace MicroserviceTemplate.Application.Features.Vehicle.UseCases;

public interface ICreateVehicleUseCase
{
    Task<Result<VehicleViewModel>> ExecuteAsync(CreateVehicleCommand command, CancellationToken cancellationToken = default);
}