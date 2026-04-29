using System.ComponentModel.DataAnnotations;

namespace CW7_Pod_kolosa.DTOs;

public class UpdateAppointmentRequestDto
{
    [Required]
    public int IdPatient { get; set; }
    
    [Required]
    public int IdDoctor { get; set; }
    
    [Required]
    public DateTime AppointmentDate { get; set; }
    
    [Required]
    public string Status { get; set; }=string.Empty;
    
    [Required]
    public string Reason { get; set; }=string.Empty;
    
    [Required]
    public string? InternalNotes { get; set; }=string.Empty;
    
}