using com.Github.Haseoo.DASPP.CoreData.Dtos;
using FluentValidation;

namespace com.Github.Haseoo.DASPP.CoreData.Validators
{
    public class FindBestVertexRequestDtoValidator : AbstractValidator<FindBestVertexRequestDto>
    {
        public FindBestVertexRequestDtoValidator()
        {
            RuleFor(x => x.BeginVertexIndex).GreaterThanOrEqualTo(0)
                .WithMessage("Beginning vertex index must be positive");
            RuleFor(x => x.EndVertexIndex).GreaterThanOrEqualTo(0)
                .WithMessage("End vertex index must be positive");
            RuleFor(x => x).Must(x => x.EndVertexIndex >= x.BeginVertexIndex)
                .WithMessage("End vertex index mus be greater than or equal to beginning index.");
        }
    }
}