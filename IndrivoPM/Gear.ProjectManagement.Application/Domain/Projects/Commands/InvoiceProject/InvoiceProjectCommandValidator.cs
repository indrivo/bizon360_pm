using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.InvoiceProject
{
   internal class InvoiceProjectCommandValidator : AbstractValidator<InvoiceProjectCommand>
    {
        public InvoiceProjectCommandValidator()
        {
            RuleFor(x => x.Id)
                 .NotEmpty();

            RuleFor(x => x.ProjectId)
                .NotEmpty();
        }
    }
}
