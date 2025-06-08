using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.DTOs;

public class ParkingRecordDto
{
    [Required]
    public int VehicleId { get; set; }
    [Required]
    public int ParkingLotId { get; set; }
    [Required]
    public DateTime CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
}