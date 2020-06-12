using FluentValidation;

namespace Gear.Manager.Core.EntityServices.Departments.Queries.GetDepartmentDetails
{
    public class GetDepartmentDetailQueryValidator:AbstractValidator<GetDepartmentDetailQuery>
    {
        public GetDepartmentDetailQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
