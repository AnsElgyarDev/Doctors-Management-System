using DoctorsManagementSystem.Data;
using DoctorsManagementSystem.Dto;
using DoctorsManagementSystem.model;
using DoctorsManagementSystem.Shared.Dtos;

namespace DoctorsManagementSystem.Service;

class AppointmentServices : IAppointmentServices
{
    private readonly AppDbContext _Context;

    public AppointmentServices(AppDbContext context)
    {
        _Context = context;
    }

    public int GetTodayAppointmentsCount()
    {
        var today = DateTime.Today;
    return _Context.Appointments
            .AsEnumerable() 
            .Count(a => a.AppointmentDate.Year == today.Year && 
            a.AppointmentDate.Month == today.Month && 
            a.AppointmentDate.Day == today.Day);
    }

    public bool ScheduleAppointment(AppointmentDto appointmentDto)
    {
        var patient = _Context.Patients.Find(appointmentDto.PatientId);
        if (patient is null)
            return false;

        var appointmentToAdd = new Appointment
        {
            PatientId = appointmentDto.PatientId,
            patient = patient,
            AppointmentDate = appointmentDto.AppointmentDate,
            AppointmentTime = appointmentDto.AppointmentTime,
            ReasonForVisit = appointmentDto.ReasonForVisit,
            Status = AppointmentStatus.Pending
        };

        _Context.Appointments.Add(appointmentToAdd);
        _Context.SaveChanges();

        return true;
    }
}