using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentsByParent
{
    internal class GetDepartmentByParentQueryValidator : AbstractValidator<GetDepartmentByParentQuery>
    {
        public GetDepartmentByParentQueryValidator()
        {
            RuleFor(x => x.BusinessUnitId).NotNull();
        }
    }
}
