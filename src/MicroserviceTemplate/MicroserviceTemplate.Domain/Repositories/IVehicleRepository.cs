using MicroserviceTemplate.Domain.Entities;

namespace MicroserviceTemplate.Domain.Repositories
{
    public interface IVehicleRepository
    {
        Task<OperationResult<Vehicle>> CreateAsync(Vehicle vehicle, CancellationToken cancellationToken = default);
    }
}
