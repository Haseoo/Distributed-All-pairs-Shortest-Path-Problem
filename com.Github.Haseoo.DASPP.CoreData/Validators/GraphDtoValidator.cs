using com.Github.Haseoo.DASPP.CoreData.Dtos;
using FluentValidation;
using System.Linq;

namespace com.Github.Haseoo.DASPP.CoreData.Validators
{
    public class GraphDtoValidator : AbstractValidator<GraphDto>
    {
        public GraphDtoValidator()
        {
            RuleFor(x => x.AdjMatrix)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Graph not provided.")
                .Must(adj => adj.Length != 0).WithMessage("Graph must have at least one vertex")
                .Must(adj =>
                {
                    var len = adj.Length;
                    return len != 0 && adj.All(row => row != null && row.Length == len);
                }).WithMessage("The adjacency matrix must be square.");
        }
    }
}