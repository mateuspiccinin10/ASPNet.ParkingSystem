using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.Models;

public class Vehicle
{
    public int Id { get; set; }
    
    [RegularExpression(@"^[A-Z]{3}-\d{4}$|^[A-Z]{3}-\d[A-Z]\d{2}$|^[A-Z]{3} \d{3}$", ErrorMessage = "A placa deve estar no formato ABC-1234, ABC-1B23 ou ABC 123")]
    public string LicensePlate { get; set; } = string.Empty;
    
    [RegularExpression("^(Carro|Moto)$", ErrorMessage = "O tipo deve ser 'Carro' ou 'Moto'")]
    public string Type { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
}