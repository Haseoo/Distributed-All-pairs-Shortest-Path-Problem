using com.Github.Haseoo.DASPP.CoreData.Validators;
using FluentValidation;

namespace com.Github.Haseoo.DASPP.Main.Dtos.Validators
{
    public class MainTaskRequestDtoValidator : AbstractValidator<MainTaskRequestDto>
    {
        public MainTaskRequestDtoValidator()
        {
            RuleFor(x => x.Granulation).GreaterThanOrEqualTo(0)
                .WithMessage("Granulation must be positive");
            RuleFor(x => x.GraphDto).NotNull()
                .WithMessage("Graph must be provided!")
                .SetValidator(new GraphDtoValidator())
                .When(x => x.GraphDto != null);
        }
    }
}