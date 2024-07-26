using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceTemplate.Infra.Data.Extensions;

public static class EntityFrameworkExtensions
{
    public static void ConfigureSqlServer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApiDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
    }
}