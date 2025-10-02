﻿using FluentValidation;

namespace Application.Entities.Travels.Commands;

public class CreateTravelCommandValidator : AbstractValidator<CreateTravelCommand>
{
    public CreateTravelCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must be less than 200 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must be less than 1000 characters.");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate)
            .WithMessage("Start date must be earlier than or equal to End date.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
