using MicroserviceTemplate.Domain.Entities;
using MicroserviceTemplate.Domain.Repositories;

namespace MicroserviceTemplate.Infra.Data.Repositories
{
    public class VehicleRepository(ApiDbContext dbContext) : RepositoryBase<ApiDbContext>(dbContext), IVehicleRepository
    {
        public async Task<OperationResult<Vehicle>> CreateAsync(Vehicle vehicle, CancellationToken cancellationToken = default)
        {
            return await TryCreateAsync(vehicle, cancellationToken);
        }
    }
}
