﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.Interfaces;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.BulkActions.BulkUpdateStatus
{
    public class BulkUpdateProjectStatusCommandHandler : IRequestHandler<BulkUpdateProjectStatusCommand, Unit>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public BulkUpdateProjectStatusCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(BulkUpdateProjectStatusCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();
            var projects = new List<Project>();

            foreach (var id in request.Projects)
            {
                var project = await _context.Projects.FindAsync(id);

                if (project == null)
                {
                    throw new NotFoundException(nameof(Project), id);
                }

                project.Status = request.Status;
                
                projects.Add(project);
            }

            if (projects.Any())
            {
                _context.Projects.UpdateRange(projects);

                await _context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
