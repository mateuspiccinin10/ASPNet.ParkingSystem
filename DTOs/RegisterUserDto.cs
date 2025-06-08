using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.Dtos;

public class RegisterUserDto
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(admin|operator)$", ErrorMessage = "O tipo de usuário deve ser 'admin' ou 'operator'")]
    public string Role { get; set; } = "user";
}