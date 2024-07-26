using FluentValidation;

namespace MicroserviceTemplate.Application.Features.Vehicle.Create;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(v => v.Vin)
            .NotEmpty()
            .MaximumLength(17);
    }
}