using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gear.CloudStorage.Abstractions.Infrastructure
{
    public interface ICloudStorageDb
    {
        DbSet<IdentityUserToken<Guid>> AspNetUserTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}
