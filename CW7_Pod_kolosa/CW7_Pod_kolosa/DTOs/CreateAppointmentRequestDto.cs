using System.ComponentModel.DataAnnotations;

namespace CW7_Pod_kolosa.DTOs;

public class CreateAppointmentRequestDto
{
    [Required]
    public int IdPatient { get; set; }
    
    [Required]
    public int IdDoctor { get; set; }
    
    [Required]
    public DateTime AppointmentDate { get; set; }
    
    [Required]
    public string Reason { get; set; }=string.Empty;
}