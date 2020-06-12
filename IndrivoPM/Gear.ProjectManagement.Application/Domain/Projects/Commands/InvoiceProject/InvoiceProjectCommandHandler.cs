using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.UserInjection;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities.Settings;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.InvoiceProject
{
    internal class InvoiceProjectCommandHandler : IRequestHandler<InvoiceProjectCommand>
    {
        private readonly IGearContext _context;
        private readonly IUserAccessor _userAccessor;

        public InvoiceProjectCommandHandler(IGearContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }

        public async Task<Unit> Handle(InvoiceProjectCommand request, CancellationToken cancellationToken)
        {
            var user = _userAccessor.GetUser();

            var entity = await _context.ProjectInvoices.FirstAsync(x => x.ProjectId == request.ProjectId, cancellationToken: cancellationToken);

            if (entity == null) throw new NotFoundException(nameof(ProjectInvoice), request.Id);

            entity.BudgetMoney = request.BudgetMoney;
            entity.BudgetMoneyHours = request.BudgetMoneyHours;
            entity.Currency = request.Currency;
            entity.HourRateMoney = request.HourRateMoney;
            entity.MaxLogHoursActivity = request.MaxLogHoursActivity;
            entity.MaxLogHoursDay = request.MaxLogHoursDay;

            _context.ProjectInvoices.Update(entity);

            // Sstp

/*            var project = await _context.Projects.FindAsync(request.ProjectId);

            if (entity == null) throw new NotFoundException(nameof(Project), request.ProjectId);

            project.ProductTypeId = request.ProductTypeId;
            project.ServiceTypeId = request.ServiceTypeId;
            project.ServiceTypeId = request.ServiceTypeId;
            project.SolutionTypeId = request.SolutionTypeId;
            

            _context.Projects.Update(project);*/

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
