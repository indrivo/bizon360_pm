using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Settings;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Unit>
    {
        private readonly IMediator _mediator;
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="context"></param>
        /// <param name="userAccessor"></param>
        public CreateProjectCommandHandler(IMediator mediator, IGearContext context, IUserAccessor userAccessor)
        {
            _mediator = mediator;
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entitySettings = new ProjectSettings()
            {
                Name = request.Name,
                ProjectId = request.Id,
            };

            entitySettings.CreateEnd(Guid.NewGuid(), 
                $"Settings for Project: {request.Name}", 
                Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value));


            var entity = new Project
            {
                Name = request.Name,
                ProjectGroupId = request.ProjectGroupId ?? _context.ProjectGroups.First(pg => !pg.IsDeletable).Id,
                Status = request.Status,
                Priority = request.Priority,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ProjectManagerId = request.ProjectManagerId,
                Description = request.Description,
                ProjectUrl = request.ProjectUrl,
                ProjectSettingsId = entitySettings.Id,
            };

            entity.CreateEnd(request.Id, request.Name,
                Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value));



            var userList = new List<ProjectMember>();
            if (request.ProjectMemberIds != null)
            {
                userList = request.ProjectMemberIds.Select(id => new ProjectMember()
                {
                    ProjectId = entity.Id,
                    UserId = id
                }).ToList();
            }

            await _context.ProjectSettings.AddAsync(entitySettings, cancellationToken);
            await _context.Projects.AddAsync(entity, cancellationToken);
            await _context.ProjectMembers.AddRangeAsync(userList, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            // Notifications
            var notification = new ProjectCreated
            {
                PrimaryEntityName = entity.Name,
                PrimaryEntityId = entity.Id,
                GroupEntityName = entity.Name,
                GroupEntityId = entity.Id,
                Recipients = userList.Any() ? userList.Select(pm => pm.User.Email).ToList() : new List<string>(),
                UserName = user.Identity.Name
            };

            await _mediator.Publish(notification, cancellationToken);

            return Unit.Value;
        }
    }
}
