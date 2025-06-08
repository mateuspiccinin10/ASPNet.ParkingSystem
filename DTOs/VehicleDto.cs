using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.DTOs;

public class VehicleDto
{
    [Required]
    [RegularExpression(@"^[A-Z]{3}-\d{4}$|^[A-Z]{3}-\d[A-Z]\d{2}$|^[A-Z]{3} \d{3}$", ErrorMessage = "A placa deve estar no formato ABC-1234, ABC-1B23 ou ABC 123")]
    public string LicensePlate { get; set; } = string.Empty;
    [Required]
    public string Type { get; set; } = string.Empty;
    [Required(ErrorMessage = "Campo obrigatório")]
    public string Brand { get; set; } = string.Empty;
    [Required(ErrorMessage = "Campo obrigatório")]
    public string Model { get; set; } = string.Empty;
    [Required(ErrorMessage = "Campo obrigatório")]
    public string Color { get; set; } = string.Empty;
}