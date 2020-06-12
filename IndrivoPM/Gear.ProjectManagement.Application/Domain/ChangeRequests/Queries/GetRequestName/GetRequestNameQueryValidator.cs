using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetRequestName
{
    public class GetRequestNameQueryValidator : AbstractValidator<GetRequestNameQuery>
    {
        public GetRequestNameQueryValidator()
        {
            RuleFor(request => request.Id).NotNull().NotEmpty();
        }
    }
}
