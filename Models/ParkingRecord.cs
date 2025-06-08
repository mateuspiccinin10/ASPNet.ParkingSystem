namespace ParkingSystem.Models;

public class ParkingRecord
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public int ParkingLotId { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }

    public Vehicle Vehicle { get; set; } = null!;
    public ParkingLot ParkingLot { get; set; } = null!;
}