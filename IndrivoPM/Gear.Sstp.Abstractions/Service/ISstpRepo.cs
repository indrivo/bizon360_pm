using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gear.Sstp.Abstractions.Domain;

namespace Gear.Sstp.Abstractions.Service
{
    public interface ISstpRepo<TEntity>
        where TEntity : SstpBaseModel
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> GetById(Guid id);

        Task Create(TEntity entity);

        Task Update(TEntity entity);

        /// <summary>
        /// Activation a list of entities.
        /// </summary>
        /// <param name="ids"> List of ids</param>
        /// <returns></returns>
        Task<bool> Activate(List<Guid> ids);

        /// <summary>
        /// Deactivation a list of entites.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> Deactivate(List<Guid> ids);

        /// <summary>
        /// Changes order of table in database to order that receive the list
        /// </summary>
        /// <param name="entityIds">List of entity ids from table</param>
        /// <returns></returns>
        Task UpdateRowOrderAsync(IList<Guid> entityIds);

        Task Delete(Guid id);
    }
}
