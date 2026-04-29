using CW7_Pod_kolosa.DTOs;
using CW7_Pod_kolosa.Exceptions;
using CW7_Pod_kolosa.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW7_Pod_kolosa.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IDbService _dbService;
    public AppointmentsController(IDbService dbService)
    {
        _dbService = dbService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll(string? status, string? patientLastName)
    {
        try
        {
            var result=await _dbService.GetAppointmentsAsync(status, patientLastName);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [Route("{idAppointment:int}")]
    [HttpGet]
    public async Task<IActionResult> GetById(int idAppointment)
    {
        try
        {
            var result=await _dbService.GetAppointmentDetailsAsync(idAppointment);
            return Ok(result);
        }
        catch (NotFoundException e)
        {
            return  NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentRequestDto dto)
    {
        try
        {
            var id = await _dbService.CreateAsync(dto);
            return Created($"api/appointments/{id}", new { idAppointment = id });
        }
        catch (NotFoundException e)
        {
            return Conflict(e.Message);
        }
    }
    
    [Route("{idAppointment:int}")]
    [HttpPut]
    public async Task<IActionResult> Update([FromRoute]int idAppointment,[FromBody] UpdateAppointmentRequestDto dto)
    {
        try
        {
            await _dbService.UpdateAsync(idAppointment, dto);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [Route("{idAppointment:int}")]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute]int idAppointment)
    {
        try
        {
            await _dbService.DeleteAsync(idAppointment);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return Conflict(e.Message);
        }
    }
    
    
}