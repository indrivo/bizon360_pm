using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Manager.Core.EntityServices.ApplicationUsers.Querries.GetApplicationUserList;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDetails
{
    public class GetActivityDetailQueryHandler : IRequestHandler<GetActivityDetailQuery, ActivityDetailModel>
    {
        private readonly IGearContext _context;

        public GetActivityDetailQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ActivityDetailModel> Handle(GetActivityDetailQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Activities
                .Include(a => a.Assignees).ThenInclude(aa => aa.User).ThenInclude(u => u.JobPosition)
                .Include(a => a.Project).ThenInclude(x => x.ProjectSettings)
                .Include(a => a.ActivityType)
                .Include(a => a.Sprint)
                .Include(a => a.ActivityList)
                .Include(a => a.LoggedTimes)
                .FirstAsync(a => a.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Activity), request.Id);
            }

            var result = ActivityDetailModel.Create(entity);
            result.Author = ApplicationUserLookupModel.Create(_context.ApplicationUsers.Find(entity.CreatedBy));

            if (request.DisplayAuditInfo != true) return result;

            result.AuditChanges = await _context.AuditLogs
                .Where(x => Guid.Parse(x.EntityPk) == request.Id
                            && x.TableName == "Activities").Select(log => new ActivityAuditModel
                {
                    EmployeeId = log.AuditUserId,
                    EmployeeName = log.AuditUserName,
                    ModificationTime = log.AuditDate,
                    Data = JsonConvert.DeserializeObject<EntityDto>(log.AuditData)
                }).ToListAsync(cancellationToken);

            return result;
        }
    }
}
