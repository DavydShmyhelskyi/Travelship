using Application.Entities.Places.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Placea.Commands
{
    public class UpdatePlaceCommandValidator : AbstractValidator<UpdatePlaceCommand>
    {
        public UpdatePlaceCommandValidator()
        {
            RuleFor(v => v.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters.")
                .MaximumLength(255).WithMessage("Title must be less then 255 characters");
            RuleFor(v => v.Latitude)
                .NotNull().WithMessage("Latitude is required.")
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");
            RuleFor(v => v.Longitude)
                .NotNull().WithMessage("Longitude is required.")
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
        }
    }
}
