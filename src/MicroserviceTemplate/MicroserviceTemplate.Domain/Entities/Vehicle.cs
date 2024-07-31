namespace MicroserviceTemplate.Domain.Entities;

public class Vehicle(string vin) : BaseEntity<long>
{
    public string Vin { get; init; } = vin;
    public string Make { get; set; }
    public string Model { get; set; }
    public string Condition { get; set; }
    public string DealerName { get; set; }
    public int Year { get; set; }
    public int ProviderId { get; set; }
}