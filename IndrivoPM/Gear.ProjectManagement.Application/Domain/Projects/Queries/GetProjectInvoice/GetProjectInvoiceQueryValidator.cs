using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectInvoice
{
    internal class GetProjectInvoiceQueryValidator : AbstractValidator<GetProjectInvoiceQuery>
    {
        public GetProjectInvoiceQueryValidator()
        {
            RuleFor(x => x.ProjectId).NotNull().NotEmpty();
        }
    }
}
