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
public class ParkingLotController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public ParkingLotController (AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    // POST
    [HttpPost]
    public async Task<IActionResult> Post(ParkingLotDto dto)
    {
        var country = StringHelper.ToTitleCase(dto.Country);

        if (country != "Brasil" && country != "Argentina")
            return BadRequest("País inválido. Somente 'Brasil' ou 'Argentina' são permitidos.");

        string state;
        if (country == "Brasil")
        {
            state = dto.State.Trim().ToUpper();
            if (state.Length != 2)
                return BadRequest("Para o Brasil, o estado deve conter exatamente 2 letras maiúsculas.");
        }
        else 
        {
            state = StringHelper.ToTitleCase(dto.State.Trim());
            if (state.Length > 20)
                return BadRequest("Para a Argentina, o estado deve ter no máximo 20 caracteres.");
        }

        var parkingLot = new ParkingLot
        {
            Name = StringHelper.ToTitleCase(dto.Name),
            Country = country,
            State = state,
            City = StringHelper.ToTitleCase(dto.City)
        };

        _appDbContext.ParkingLots.Add(parkingLot);
        await _appDbContext.SaveChangesAsync();

        return Created("Estacionamento adicionado com sucesso.", parkingLot);
    }
    
    //GET ALL
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var parkingLots = await _appDbContext.ParkingLots.ToListAsync();
        return Ok(parkingLots);
    }
    
    
    // GET BY ID
    [HttpGet("{id}")]
    public async Task<ActionResult<ParkingLot>> GetById(int id)
    {
        var parkingLot = await _appDbContext.ParkingLots.FindAsync(id);

        if (parkingLot == null)
            return NotFound("Estacionamento não encontrado.");

        return Ok(parkingLot);
    }
    

    // PUT
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ParkingLotDto dto)
    {
        var parkingLot = await _appDbContext.ParkingLots.FindAsync(id);
        if (parkingLot == null)
            return NotFound("Estacionamento não encontrado.");

        var country = StringHelper.ToTitleCase(dto.Country);

        if (country != "Brasil" && country != "Argentina")
            return BadRequest("País inválido. Somente 'Brasil' ou 'Argentina' são permitidos.");

        string state;
        if (country == "Brasil")
        {
            state = dto.State.Trim().ToUpper();
            if (state.Length != 2)
                return BadRequest("Para o Brasil, o estado deve conter exatamente 2 letras.");
        }
        else
        {
            state = StringHelper.ToTitleCase(dto.State.Trim());
            if (state.Length > 20)
                return BadRequest("Para a Argentina, o estado deve ter no máximo 20 caracteres.");
        }

        parkingLot.Name = StringHelper.ToTitleCase(dto.Name);
        parkingLot.Country = country;
        parkingLot.State = state;
        parkingLot.City = StringHelper.ToTitleCase(dto.City);

        await _appDbContext.SaveChangesAsync();
        return StatusCode(201, parkingLot);
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var parkingLot = await _appDbContext.ParkingLots.FindAsync(id);
        if (parkingLot == null)
            return NotFound("Estacionamento não encontrado.");

        _appDbContext.ParkingLots.Remove(parkingLot);
        await _appDbContext.SaveChangesAsync();
        return Ok("Estacionamento deletado com sucesso.");
    }
}