﻿using FluentValidation;
using RuiSantos.ZocDoc.Core.Models;

namespace RuiSantos.ZocDoc.Core.Validators;

public class MedicalSpecialityValidator : AbstractValidator<MedicalSpeciality>
{
    public readonly static MedicalSpecialityValidator Instance = new MedicalSpecialityValidator();

    public MedicalSpecialityValidator()
    {
        RuleFor(model => model.Description).NotEmpty();
    }
}