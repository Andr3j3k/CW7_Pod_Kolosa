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
    
}