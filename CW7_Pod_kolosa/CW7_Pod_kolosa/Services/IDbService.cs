using CW7_Pod_kolosa.DTOs;

namespace CW7_Pod_kolosa.Services;

public interface IDbService
{
    Task<AppointmentListDto> GetAppointmentsAsync(string? status, string? patientLastName);
    Task<AppointmentDetailsDto> GetAppointmentDetailsAsync(int idAppointment);
    Task<int> CreateAsync(CreateAppointmentRequestDto dto);
    Task UpdateAsync(int idAppointment, UpdateAppointmentRequestDto dto);
}