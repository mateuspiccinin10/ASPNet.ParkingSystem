using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.DTOs
{
    public class ParkingLotDto
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Country { get; set; } = string.Empty;
        [Required(ErrorMessage = "Campo obrigatório")]
        public string State { get; set; } = string.Empty;
        [Required(ErrorMessage = "Campo obrigatório")]
        public string City { get; set; } = string.Empty;
    }
}