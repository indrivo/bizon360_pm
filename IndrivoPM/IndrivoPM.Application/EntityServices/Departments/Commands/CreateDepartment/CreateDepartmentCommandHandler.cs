using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.HrmEntities;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.Manager.Core.EntityServices.Departments.Commands.CreateDepartment
{
    public class CreateDepartmentCommandHandler: IRequestHandler<CreateDepartmentCommand, Unit>
    {
        private readonly IUserAccessor _userAccessor;
        private readonly IGearContext _context;
        private readonly IMediator _mediator;

        public CreateDepartmentCommandHandler(IGearContext context, IUserAccessor userAccessor, IMediator mediator)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var userId = Guid.Parse(_userAccessor.GetUser().FindFirst(ClaimTypes.NameIdentifier).Value);

            var department = new Department
            {
                Name = request.Name,
                Description = request.Description,
                BusinessUnitId = request.BusinessUnitId,
                DepartmentLeadId = request.DepartmentLeadId,
                Abbreviation = request.Abbreviation
            };

            department.CreateEnd(Guid.NewGuid(), request.Name, userId);

            if (request.Active)
                department.Activate();
            else
                department.Deactivate();

            _context.Departments.Add(department);

            await _context.SaveChangesAsync(cancellationToken);


            var notification = new DepartmentCreated()
            {
                PrimaryEntityId = department.Id,
                PrimaryEntityName = department.Name,
                GroupEntityId = department.Id,
                GroupEntityName = department.Name,
                UserName = user.Identity.Name
            };

            await _mediator.Publish(notification);

            return Unit.Value;
        }
    }
}
