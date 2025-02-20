﻿namespace RuiSantos.ZocDoc.Core.Models;

/// <summary>
/// Represents a medical speciality
/// </summary>
public class MedicalSpecialty
{
    /// <summary>
    /// The id of the medical speciality
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// The description of the medical speciality
    /// </summary>
    public string Description { get; init; }

    /// <summary>
    /// Create a new instance of the medical speciality
    /// </summary>
    public MedicalSpecialty()
    {
        Id = Guid.NewGuid();
        Description = String.Empty;
    }

    /// <summary>
    /// Create a new instance of the medical speciality
    /// </summary>
    /// <param name="description">The description of the medical speciality</param>
    public MedicalSpecialty(string description)
    {
        Id = Guid.NewGuid();
        Description = description;
    }
}

