using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gear.Sstp.Abstractions.Domain;
using Gear.Sstp.Abstractions.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Gear.Sstp.Abstractions.Service
{
    public class SstpRepo<TEntity> : ISstpRepo<TEntity>
        where TEntity : SstpBaseModel
    {
        private readonly ISstpContext _context;
        

        public SstpRepo(ISstpContext context)
        {
            _context = context;
        }

        public virtual async Task Create(TEntity entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedTime = DateTime.Now;
            entity.ModifiedTime = DateTime.Now;
            await _context.Instance.Set<TEntity>().AddAsync(entity);
            await _context.Instance.SaveChangesAsync(CancellationToken.None);
        }

        public virtual async Task Update(TEntity entity)
        {
            entity.ModifiedTime = DateTime.Now;
            _context.Instance.Set<TEntity>().Update(entity);
            await _context.Instance.SaveChangesAsync(CancellationToken.None);
        }

        /// <summary>
        /// Changes order of table in database to order that receive the list
        /// </summary>
        /// <param name="entityIds">List of entity ids from html table</param>
        /// <returns></returns>
        public virtual async Task UpdateRowOrderAsync(IList<Guid> entityIds)
        {
            if (entityIds == null) throw new IdNullOrEmptyException();

            var count = 1;

            foreach (var item in entityIds)
            {
                var entity = await _context.Instance.Set<TEntity>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == item);

                if (entity == null) throw new NotFoundException(nameof(TEntity), item);

                entity.RowOrder = count;

                _context.Instance.Set<TEntity>().Update(entity);

                count++;
            }

            await _context.Instance.SaveChangesAsync(CancellationToken.None);
        }

        public virtual async Task Delete(Guid id)
        {
            var entity = await GetById(id);
            _context.Instance.Set<TEntity>().Remove(entity);
            await _context.Instance.SaveChangesAsync(CancellationToken.None);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return  _context.Instance.Set<TEntity>().OrderBy(x => x.RowOrder);
        }

        public virtual async Task<TEntity> GetById(Guid id)
        {
            try
            {
                return await _context.Instance.Set<TEntity>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == id);
            }
            catch
            {
                throw new NotFoundException("Entity not found", id);
            }
        }

        public virtual async Task<bool> CheckIfExists(Guid id)
        {
            return await _context.Instance.Set<TEntity>().AnyAsync(x => x.Id == id);
        }

        public async Task<bool> Activate(List<Guid> ids)
        {
            if (ids.Count == 0 || ids == null) throw new IdNullOrEmptyException();

            foreach(var item in ids)
            {
                if (await CheckIfExists(item))
                {
                    var entity = await GetById(item);
                    entity.Active = true;
                    entity.ModifiedTime = DateTime.Now;

                    _context.Instance.Set<TEntity>().Update(entity);
                }
            }

            bool success = await _context.Instance.SaveChangesAsync(CancellationToken.None) > 0;

            return success;
        }

        public async Task<bool> Deactivate(List<Guid> ids)
        {
            if (ids.Count == 0 || ids == null) throw new IdNullOrEmptyException();

            foreach (var item in ids)
            {
                if (await CheckIfExists(item))
                {
                    var entity = await GetById(item);

                    if (entity.IsDeletable)
                    {
                         entity.Active = false;
                         entity.ModifiedTime = DateTime.Now;

                         _context.Instance.Set<TEntity>().Update(entity);
                    }
                }
            }

            bool success = await _context.Instance.SaveChangesAsync(CancellationToken.None) > 0;

            return success;
        }
    }
}
