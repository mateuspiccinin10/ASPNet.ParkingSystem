using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    [Required]
    public string Role { get; set; } = "admin";
    public bool IsActive { get; set; } = true;
}