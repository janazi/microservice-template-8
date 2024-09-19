using Asp.Versioning;
using MicroserviceTemplate.Application.Features.Vehicle;
using MicroserviceTemplate.Application.Features.Vehicle.Create;
using MicroserviceTemplate.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MicroserviceTemplate.Api.Controllers.v2;

[ApiVersion("2.0")]
[ApiController]
[Route("api/{version:apiVersion}/[controller]")]
public partial class VehiclesController : ControllerBase
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
        if (result.IsSuccess)
            return Ok(result);

        return UnprocessableEntity(result.Error);
    }

    [HttpGet("{vehicleId}")]
    [ProducesResponseType(typeof(VehicleViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute] Guid vehicleId)
    {
        return Ok(new { vehicleId });
    }
}