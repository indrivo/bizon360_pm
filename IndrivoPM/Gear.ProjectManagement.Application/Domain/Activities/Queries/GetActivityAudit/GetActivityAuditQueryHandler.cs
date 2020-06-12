using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Entities;
using Gear.Domain.Infrastructure;
using Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Activities.Queries.GetActivityAudit
{
    public class GetActivityAuditQueryHandler : IRequestHandler<GetActivityAuditQuery, ActivityAuditListView>
    {
        private readonly IGearContext _context;

        public GetActivityAuditQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ActivityAuditListView> Handle(GetActivityAuditQuery request, CancellationToken cancellationToken)
        {

            var timer = new Stopwatch();
            timer.Start();

            var result =await _context.AuditLogs.Include(x => x.Changes)
                .Where(x => Guid.Parse(x.EntityPk) == request.Id 
                            && x.TableName == "Activities").Select(log =>
                new ActivityAuditModel()
                {
                    EmployeeId = log.AuditUserId,
                    EmployeeName = log.AuditUserName,
                    ModificationTime = log.AuditDate,
                    Data = new EntityDto()
                    {
                        Entity = new Entity()
                        {
                            ModifiedTime = log.AuditDate,
                            Name = log.EntityTypeName,
                            ModifiedBy = log.AuditUserId
                        },
                        Action = log.Action,
                        Changes = log.Changes != null ? log.Changes.ToList() : new List<EntryChange>()
                    }
                }).AsNoTracking().ToListAsync(cancellationToken);

            timer.Stop();

            var time = timer.ElapsedMilliseconds;

            return new ActivityAuditListView
            {
                Audits = result
            };
        }
    }
}
