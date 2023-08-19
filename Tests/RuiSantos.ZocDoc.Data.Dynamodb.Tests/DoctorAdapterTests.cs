using RuiSantos.ZocDoc.Data.Dynamodb.Adapters;
using RuiSantos.ZocDoc.Data.Dynamodb.Tests.Fixtures;

namespace RuiSantos.ZocDoc.Data.Dynamodb.Tests;

public class DoctorAdapterTests: IClassFixture<DatabaseFixture>
{
    private readonly DoctorAdapter doctorAdapter;
    
    public DoctorAdapterTests(DatabaseFixture database)
    {
        this.doctorAdapter = new(database.Client);
    }

    [Fact(DisplayName = "Should find doctor by license.")]
    public async Task ShouldFindDoctorByLicenseNumber()
    {
        // Arrange
        const string licenseNumber = "XYZ002";
        const string expectedId = "d6c9f315-0e35-4d5b-b25e-61a61c92d9c9";

        // Act
        var doctor = await this.doctorAdapter.FindAsync(licenseNumber);

        // Assert
        doctor.Should().NotBeNull();
        doctor!.License.Should().Be(licenseNumber);
        doctor!.Id.Should().Be(expectedId);
    }

    [Fact(DisplayName = "Should find doctors by specialties.")]
    public async Task ShouldFindDoctorBySpecialties()
    {
        // Arrange
        const string specialty = "Dermatology";
        var expectedIds = new[] { "d6c9f315-0e35-4d5b-b25e-61a61c92d9c9", "8a6151c7-9122-4f1b-a1e7-85e981c17a14" };

        // Act
        var doctors = await this.doctorAdapter.FindBySpecialityAsync(specialty);

        // Assert
        doctors.Should().NotBeNull().And.HaveCount(2);
        doctors.Should().OnlyContain(d => expectedIds.Contains(d.Id.ToString()));
    }

    [Fact(DisplayName = "Should find doctors by specialties with availability.")]
    public async Task ShouldFindDoctorBySpecialtiesWithAvailability()
    {
        // Arrange
        const string specialty = "Dermatology";
        var desiredDate = new DateOnly(2023, 8, 21);
        
        var expectedIds = new[] { "d6c9f315-0e35-4d5b-b25e-61a61c92d9c9", "8a6151c7-9122-4f1b-a1e7-85e981c17a14" };

        // Act
        var doctors = await this.doctorAdapter.FindBySpecialtyWithAvailabilityAsync(specialty, desiredDate);

        // Assert
        doctors.Should().NotBeNull().And.HaveCount(2);
        doctors.Should().OnlyContain(d => expectedIds.Contains(d.Id.ToString()));
        doctors.Should().OnlyContain(d => d.OfficeHours.Any(a => a.Week == DayOfWeek.Monday));
    }
}