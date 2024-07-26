using FluentValidation;
using MicroserviceTemplate.Application.Features.Vehicle.Create;
using MicroserviceTemplate.Application.Features.Vehicle.UseCases;
using MicroserviceTemplate.Domain.Repositories;
using MicroserviceTemplate.Infra.Data.Repositories;
using System.Reflection;

namespace MicroserviceTemplate.Api.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureIoC(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateVehicleCommand>, CreateVehicleCommandValidator>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<ICreateVehicleUseCase, CreateVehicleUseCase>();
            services.AddAutoMapper(Assembly.Load("MicroserviceTemplate.Application"));
        }


    }
}
