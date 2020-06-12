using System;
using System.Threading;
using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Infrastructure;
using Gear.Common.Entities;
using Gear.Domain.AppEntities;
using Gear.Domain.HrmInterfaces;
using Gear.Domain.PmInterfaces;
using Gear.Domain.ReportInterfaces;
using Gear.Sstp.Abstractions.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Gear.Domain.Infrastructure
{
    /// <summary>
    /// Inverse the dependency of persistence layer on application on Entity Framework
    /// </summary>
    public interface IGearContext : IHrmContext, IPmContext, ICloudStorageDb, ISstpContext, IReportContext<Guid>
    {
        new DbContext Instance { get; }

        new DbSet<ApplicationUser> ApplicationUsers { get; set; }

        IModel Model { get; }

        DbSet<AuditLog> AuditLogs { get; set; }

        object Find(Type entityType, params object[] keyValues);

        new Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
