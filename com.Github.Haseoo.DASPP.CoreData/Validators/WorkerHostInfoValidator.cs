using com.Github.Haseoo.DASPP.CoreData.Dtos;
using FluentValidation;

namespace com.Github.Haseoo.DASPP.CoreData.Validators
{
    public class WorkerHostInfoValidator : AbstractValidator<WorkerHostInfo>
    {
        public WorkerHostInfoValidator()
        {
            CascadeMode = CascadeMode.Continue;
            RuleFor(x => x.CoreCount).GreaterThan(0).WithMessage("Core count must be greater than zero.");
            RuleFor(x => x.Uri).NotNull().NotEmpty().WithMessage("Worker uri must be provided.");
        }
    }
}