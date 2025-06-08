using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Models;
using ParkingSystem.Utils;

namespace ParkingSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public VehicleController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    // POST
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] VehicleDto dto)
    {
        bool placaExiste = await _appDbContext.Vehicles
            .AnyAsync(v => v.LicensePlate == dto.LicensePlate);

        if (placaExiste)
            return Conflict("Já existe um veículo com essa placa.");

        var formattedType = StringHelper.ToTitleCase(dto.Type);

        
        if (formattedType != "Carro" && formattedType != "Moto")
            return BadRequest("Tipo de veículo inválido. Permitido apenas: Carro ou Moto.");

        var vehicle = new Vehicle
        {
            LicensePlate = StringHelper.NormalizeUpper(dto.LicensePlate),
            Type = formattedType,
            Brand = StringHelper.ToTitleCase(dto.Brand),
            Model = StringHelper.ToTitleCase(dto.Model),
            Color = StringHelper.ToTitleCase(dto.Color)
        };

        _appDbContext.Vehicles.Add(vehicle);
        await _appDbContext.SaveChangesAsync();

        return Created("Veiculo cadastrado com sucesso.", vehicle);
    }
    
    //GET ALL
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vehicle>>> GetAll()
    {
        var vehicles = await _appDbContext.Vehicles.ToListAsync();
        return Ok(vehicles);
    }
    
    //GET BY ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Vehicle>> GetById(int id)
    {
        var vehicle = await _appDbContext.Vehicles.FindAsync(id);
    
        if (vehicle == null)
            return NotFound("Veiculo não encontrado.");

        return Ok(vehicle);
    }
    
    // PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] VehicleDto dto)
    {
        var vehicle = await _appDbContext.Vehicles.FindAsync(id);
        if (vehicle == null)
            return NotFound("Veículo não encontrado.");

        var formattedType = StringHelper.ToTitleCase(dto.Type);

        
        if (formattedType != "Carro" && formattedType != "Moto")
            return BadRequest("Tipo de veículo inválido. Permitido apenas: Carro ou Moto.");

        vehicle.LicensePlate = StringHelper.NormalizeUpper(dto.LicensePlate);
        vehicle.Type = formattedType;
        vehicle.Brand = StringHelper.ToTitleCase(dto.Brand);
        vehicle.Model = StringHelper.ToTitleCase(dto.Model);
        vehicle.Color = StringHelper.ToTitleCase(dto.Color);

        await _appDbContext.SaveChangesAsync();
        return Ok("Veículo atualizado com sucesso.");
    }
    
    //DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var vehicle = await _appDbContext.Vehicles.FindAsync(id);

        if (vehicle == null)
            return NotFound("Veiculo não encontrado.");

        _appDbContext.Vehicles.Remove(vehicle);
        await _appDbContext.SaveChangesAsync();

        return Ok("Veiculo deletado com sucesso.");
    }
    
    
}