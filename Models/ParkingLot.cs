using System.ComponentModel.DataAnnotations;

namespace ParkingSystem.Models;

public class ParkingLot
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [Required]
    [RegularExpression("^(Brasil|Argentina)$", ErrorMessage = "O país deve ser 'Brasil' ou 'Argentina'.")]
    public string Country { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    
    public ICollection<ParkingRecord> ParkingRecords { get; set; } = new List<ParkingRecord>();
}