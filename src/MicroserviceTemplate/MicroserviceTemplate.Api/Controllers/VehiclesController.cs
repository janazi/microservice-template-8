using Asp.Versioning;
using MicroserviceTemplate.Application.Features.Vehicle;
using MicroserviceTemplate.Application.Features.Vehicle.Create;
using MicroserviceTemplate.Application.Features.Vehicle.UseCases;
using MicroserviceTemplate.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceTemplate.Api.Controllers;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class VehiclesController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(VehicleViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(VehicleViewModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(VehicleViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Vehicle>> Post([FromBody] CreateVehicleCommand createVehicleCommand,
        [FromServices] ICreateVehicleUseCase createVehicleUseCase,
        CancellationToken cancellationToken)
    {
        var result = await createVehicleUseCase.ExecuteAsync(createVehicleCommand, cancellationToken);
        return result.Match<ActionResult<Vehicle>>(
            vehicle => CreatedAtAction("GetById", new { VehicleId = vehicle.Id }, vehicle),
            error => UnprocessableEntity(error));
    }

    [HttpGet("{vehicleId}")]
    [ProducesResponseType(typeof(VehicleViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute] Guid vehicleId)
    {
        throw new NotImplementedException();
    }
}