using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using RuiSantos.ZocDoc.Core.Adapters;
using RuiSantos.ZocDoc.Core.Models;
using RuiSantos.ZocDoc.Data.Dynamodb.Entities;

namespace RuiSantos.ZocDoc.Data.Dynamodb.Adapters;

public class DoctorAdapter : IDoctorAdapter
{
    private readonly IDynamoDBContext context;

    public DoctorAdapter(AmazonDynamoDBClient client)
    {
        this.context = new DynamoDBContext(client);
    }

    public async Task<Doctor?> FindAsync(string license)
        => await DoctorDto.GetDoctorByLicenseAsync(context, license);
    
    public async Task<List<Doctor>> FindBySpecialityAsync(string specialty)
        => await DoctorDto.GetDoctorsBySpecialtyAsync(context, specialty);
    
    public async Task<List<Doctor>> FindBySpecialtyWithAvailabilityAsync(string specialty, DateOnly date)
    {
        var doctors = await FindBySpecialityAsync(specialty);

        return doctors
            .Select(x => new
            {
                Doctor = x,
                OfficeHours = x.OfficeHours.Where(h => h.Week == date.DayOfWeek).SelectMany(s => s.Hours),
                Appointments = x.Appointments.Where(a => a.Date == date).Select(s => s.Time)
            })
            .Where(x => x.Appointments.Count() < x.OfficeHours.Count())
            .Select(x => x.Doctor)
            .ToList();
    }

    public async Task<List<Doctor>> FindAllWithAppointmentsAsync(IEnumerable<Appointment> appointments)
    {
        var tasks = appointments.Select(async appoint => await DoctorDto.GetDoctorByAppointmentIdAsync(context, appoint.Id));
        var result = await Task.WhenAll(tasks);
        if (result is null)
            return new List<Doctor>();

        return result.Where(i => i is not null).Select(i => i!).ToList();
    }

    public async Task StoreAsync(Doctor doctor)
    {
        await DoctorDto.SetDoctorAsync(context, doctor);
    }
}