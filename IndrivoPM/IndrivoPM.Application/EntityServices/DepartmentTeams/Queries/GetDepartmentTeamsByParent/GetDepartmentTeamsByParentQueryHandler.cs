using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Domain.Infrastructure;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.Manager.Core.EntityServices.DepartmentTeams.Queries.GetDepartmentTeamsByParent
{
   public class GetDepartmentTeamsByParentQueryHandler : IRequestHandler<GetDepartmentTeamsByParentQuery, DepartmentTeamsByParentModel>
   {
       private readonly IGearContext _context;

       public GetDepartmentTeamsByParentQueryHandler(IGearContext context)
       {
           _context = context;
       }

       public async Task<DepartmentTeamsByParentModel> Handle(GetDepartmentTeamsByParentQuery request, CancellationToken cancellationToken)
       {
           return new DepartmentTeamsByParentModel
           {
               DepartmentTeams = await _context.DepartmentTeams
                   .Where(x => x.IsDeletable && x.DepartmentId == request.DepartmentId)
                   .Include(x => x.UserDepartmentTeams).ThenInclude(x => x.User)
                   .Select(x => DepartmentTeamsByParentLookup.Create(x)).ToListAsync(cancellationToken),
               DepartmentId = request.DepartmentId
           };
       }
   }
}
