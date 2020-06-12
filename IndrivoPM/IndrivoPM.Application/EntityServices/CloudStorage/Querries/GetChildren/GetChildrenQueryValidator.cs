using FluentValidation;

namespace Gear.Manager.Core.EntityServices.CloudStorage.Querries.GetChildren
{
    public class GetChildrenQueryValidator : AbstractValidator<GetChildrenQuery>
    {
        public GetChildrenQueryValidator()
        {
            RuleFor(x => x.FilePath)
                .NotNull();

            RuleFor(x => x.ExternalProvider)
                .NotNull();
        }
    }
}
