namespace MicroserviceTemplate.Domain.Entities;

public class Vehicle(string vin) : BaseEntity<long>
{
    public string Vin { get; init; } = vin;
}