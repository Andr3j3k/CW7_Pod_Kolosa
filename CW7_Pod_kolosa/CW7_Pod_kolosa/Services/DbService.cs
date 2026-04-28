using CW7_Pod_kolosa.DTOs;
using CW7_Pod_kolosa.Exceptions;
using Microsoft.Data.SqlClient;

namespace CW7_Pod_kolosa.Services;

public class DbService : IDbService
{
    private readonly string _connectionString;

    public DbService(IConfiguration config)
    {
        _connectionString=config.GetConnectionString("DefaultConnection") ?? string.Empty;
    }

    public async Task<AppointmentListDto> GetAppointmentsAsync(string? status, string? patientLastName)
    {
        var query = """
                    SELECT
                        a.IdAppointment,
                        a.AppointmentDate,
                        a.Status,
                        a.Reason,
                        p.FirstName + N' ' + p.LastName AS PatientFullName,
                        p.Email AS PatientEmail
                    FROM dbo.Appointments a
                    JOIN dbo.Patients p ON p.IdPatient = a.IdPatient
                    WHERE (@Status IS NULL OR a.Status = @Status)
                      AND (@PatientLastName IS NULL OR p.LastName = @PatientLastName)
                    ORDER BY a.AppointmentDate;
                    """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText=query;
        command.Parameters.AddWithValue("@Status", status);
        command.Parameters.AddWithValue("@PatientLastName", patientLastName);

        await using var reader = await command.ExecuteReaderAsync();
        
        AppointmentListDto? result=null;

        var ordIdAppointment = reader.GetOrdinal("IdAppointment");
        var ordAppointmentDate=reader.GetOrdinal("AppointmentDate");
        var ordStatus=reader.GetOrdinal("Status");
        var ordReason=reader.GetOrdinal("Reason");
        var ordPatientFullName=reader.GetOrdinal("PatientFullName");
        var ordPatientEmail=reader.GetOrdinal("PatientEmail");

        while (await reader.ReadAsync())
        {
            if (result is null)
            {
                result = new AppointmentListDto()
                {
                    IdAppointment = reader.GetInt32(ordIdAppointment),
                    AppointmentDate = reader.GetDateTime(ordAppointmentDate),
                    Status = reader.GetString(ordStatus),
                    Reason = reader.GetString(ordReason),
                    PatientFullName = reader.GetString(ordPatientFullName),
                    PatientEmail = reader.GetString(ordPatientEmail)
                };
            }
        }
        return result ?? throw new NotFoundException("Appointment not found");
    }

    public async Task<AppointmentDetailsDto> GetAppointmentDetailsAsync(int idAppointment)
    {
        var query = """
                    SELECT 
                    a.IdAppointment,
                    a.AppointmentDate,
                    a.Status,
                    a.Reason,
                    a.InternalNotes,
                    a.CreatedAt,
                    p.FirstName + ' ' + p.LastName AS PatientFullName,
                    p.Email,
                    p.PhoneNumber,
                    d.FirstName + ' ' + d.LastName AS DoctorFullName,
                    d.LicenseNumber,
                    s.Name
                    FROM Appointments a
                    INNER JOIN Patients p ON a.IdPatient=p.IdPatient
                    INNER JOIN Doctors d ON d.IdDoctor=a.IdDoctor
                    INNER JOIN Specializations s ON s.IdSpecialization=d.IdSpecialization
                    WHERE a.IdAppointment=@idAppointment
                    """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText=query;
        command.Parameters.AddWithValue("@idAppointment", idAppointment);

        await using var reader = await command.ExecuteReaderAsync();
        
        AppointmentDetailsDto? result=null;
        
        var ordIdAppointment=reader.GetOrdinal("IdAppointment");
        var ordAppointmentDate=reader.GetOrdinal("AppointmentDate");
        var ordStatus=reader.GetOrdinal("Status");
        var ordReason=reader.GetOrdinal("Reason");
        var ordInternalNotes=reader.GetOrdinal("InternalNotes");
        var ordCreatedAt=reader.GetOrdinal("CreatedAt");
        var ordPatientFullName=reader.GetOrdinal("PatientFullName");
        var ordEmail=reader.GetOrdinal("Email");
        var ordPhoneNumber=reader.GetOrdinal("PhoneNumber");
        var ordDoctorFullName=reader.GetOrdinal("DoctorFullName");
        var ordLicenseNumber=reader.GetOrdinal("LicenseNumber");
        var ordName=reader.GetOrdinal("Name");

        while (await reader.ReadAsync())
        {
            if (result is null)
            {
                result = new AppointmentDetailsDto()
                {
                    IdAppointment = reader.GetInt32(ordIdAppointment),
                    AppointmentDate = reader.GetDateTime(ordAppointmentDate),
                    Status = reader.GetString(ordStatus),
                    Reason = reader.GetString(ordReason),
                    InternalNotes = reader.IsDBNull(ordInternalNotes)
                        ? null
                        : reader.GetString(ordInternalNotes),
                    CreatedAt = reader.GetDateTime(ordCreatedAt),
                    PatientFullName = reader.GetString(ordPatientFullName),
                    PatientEmail = reader.GetString(ordEmail),
                    PatientPhoneNumber = reader.GetString(ordPhoneNumber),
                    DoctorFullName = reader.GetString(ordDoctorFullName),
                    DoctorLicenseNumber = reader.GetString(ordLicenseNumber),
                    SpecializationName = reader.GetString(ordName),
                };
            }
        }
        return result ?? throw new NotFoundException("Appointment not found");
    }
}