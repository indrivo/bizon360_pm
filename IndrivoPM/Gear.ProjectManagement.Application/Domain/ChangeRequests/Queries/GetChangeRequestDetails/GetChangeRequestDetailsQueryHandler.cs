using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.Common.Exceptions;
using Gear.Domain.Infrastructure;
using Gear.Domain.PmEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Manager.Core.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gear.ProjectManagement.Manager.Domain.ChangeRequests.Queries.GetChangeRequestDetails
{
    public class GetChangeRequestDetailsQueryHandler : IRequestHandler<GetChangeRequestDetailsQuery, ChangeRequestDetailModel>
    {
        private readonly IGearContext _context;

        public GetChangeRequestDetailsQueryHandler(IGearContext context)
        {
            _context = context;
        }

        public async Task<ChangeRequestDetailModel> Handle(GetChangeRequestDetailsQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ChangeRequests
                .Include(cr => cr.Project)
                .FirstAsync(cr => cr.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ChangeRequest), request.Id);
            }

            var model = ChangeRequestDetailModel.Create(entity);

            var author = await _context.ApplicationUsers
                .Include(u => u.JobPosition)
                .FirstAsync(u => u.Id == entity.CreatedBy, cancellationToken);

            if (model.Status == ChangeRequestStatus.Refused || model.Status == ChangeRequestStatus.Approved)
            {
                if (entity.ReviewBy != Guid.Empty)
                {
                    var reviewer = await _context.ApplicationUsers
                        .FirstAsync(u => u.Id == entity.ReviewBy, cancellationToken);


                    model.ReviewById = reviewer.Id;
                    model.ReviewByName = $"{reviewer.FirstName} {reviewer.LastName}";
                }

            }


            model.AuthorId = author.Id;
            model.AuthorName = $"{author.FirstName} {author.LastName}";
            model.AuthorJobPosition = author.JobPosition != null ? author.JobPosition.Name : "Not Specified";

            return model;
        }
    }
}
