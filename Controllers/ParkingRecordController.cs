using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingSystem.Data;
using ParkingSystem.DTOs;
using ParkingSystem.Models;

namespace ParkingSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ParkingRecordController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public ParkingRecordController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    //POST
    [HttpPost]
    public async Task<IActionResult> Post(ParkingRecordDto dto)
    {
        if (dto.CheckOut.HasValue && dto.CheckOut <= dto.CheckIn)
        {
            return BadRequest(new { message = "O Check-out deve ser posterior ao Check-in." });
        }

        var record = new ParkingRecord
        {
            VehicleId = dto.VehicleId,
            ParkingLotId = dto.ParkingLotId,
            CheckIn = dto.CheckIn,
            CheckOut = dto.CheckOut
        };

        _appDbContext.ParkingRecords.Add(record);
        await _appDbContext.SaveChangesAsync();

        return Created("Registro criado com sucesso.", record);
    }
    
    //GET ALL
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _appDbContext.ParkingRecords
            .Include(r => r.Vehicle)
            .Include(r => r.ParkingLot)
            .ToListAsync();

        var records = data.Select(r => new
        {
            r.Id,
            CheckIn = r.CheckIn.ToString("yyyy-MM-dd HH:mm"),
            CheckOut = r.CheckOut?.ToString("yyyy-MM-dd HH:mm"),
            Vehicle = new
            {
                r.Vehicle.LicensePlate,
                r.Vehicle.Type,
                r.Vehicle.Model,
                r.Vehicle.Color
            },
            ParkingLot = new
            {
                r.ParkingLot.Name,
                r.ParkingLot.Country,
                r.ParkingLot.State,
                r.ParkingLot.City
            }
        }).ToList();

        return Ok(records);
    }
    
    //GET BY ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _appDbContext.ParkingRecords
            .Include(r => r.Vehicle)
            .Include(r => r.ParkingLot)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (record == null)
            return NotFound("Registro não encontrado.");

        var result = new
        {
            record.Id,
            CheckIn = record.CheckIn.ToString("yyyy-MM-dd HH:mm"),
            CheckOut = record.CheckOut?.ToString("yyyy-MM-dd HH:mm"),
            Vehicle = new
            {
                record.Vehicle.LicensePlate,
                record.Vehicle.Type,
                record.Vehicle.Model,
                record.Vehicle.Color
            },
            ParkingLot = new
            {
                record.ParkingLot.Name,
                record.ParkingLot.Country,
                record.ParkingLot.State,
                record.ParkingLot.City
            }
        };

        return Ok(result);
    }
    
    //PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] ParkingRecordDto dto)
    {
        var record = await _appDbContext.ParkingRecords.FindAsync(id);

        if (record == null)
            return NotFound("Registro não encontrado." );

        if (dto.CheckOut.HasValue && dto.CheckOut <= dto.CheckIn)
            return BadRequest("O Check-out deve ser posterior ao Check-in." );

        record.VehicleId = dto.VehicleId;
        record.ParkingLotId = dto.ParkingLotId;
        record.CheckIn = dto.CheckIn;
        record.CheckOut = dto.CheckOut;

        await _appDbContext.SaveChangesAsync();
        return StatusCode(201, record);
    }
    
    //DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var record = await _appDbContext.ParkingRecords.FindAsync(id);

        if (record == null)
            return NotFound("Registro não encontrado." );

        _appDbContext.ParkingRecords.Remove(record);
        await _appDbContext.SaveChangesAsync();

        return Ok("Registro deletado com sucesso.");
    }
    
    
    
}