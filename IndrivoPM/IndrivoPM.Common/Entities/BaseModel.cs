using System;
using Gear.Common.Exceptions;
using Gear.Common.Extensions.Result;

namespace Gear.Common.Entities
{
    public class BaseModel
    {
        public Guid Id { get; private set; } = Guid.Empty;

        public string Name { get; set; }

        public Guid CreatedBy { get; private set; }

        public System.DateTime CreatedTime { get; private set; } = System.DateTime.Now;

        public Guid ModifiedBy { get; private set; }

        public System.DateTime ModifyTime { get; private set; } = System.DateTime.Now;

        public bool IsDeletable { get; set; } = true;

        public bool Active { get; private set; } = true;

        public BaseModel(){}

        public BaseModel(Guid id)
        {
            if (id == Guid.Empty)
                throw new NotFoundException(nameof(this.GetType), id);
            Id = id;
        }

        /// <summary>
        /// Method added to exclude the constructor building.
        /// TODO: Change for a proper Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public void CreateEnd(Guid id, string name, Guid createdBy)
        {
            if (id == Guid.Empty)
                throw new InvalidOperationException("Id should not be empty");

            this.Id = id;

            if (string.IsNullOrEmpty(name.Trim()))
                throw new InvalidOperationException("Name should not be empty");
            this.Name = name != "" ? name : "No Name Provided";

            if (createdBy == Guid.Empty)
            {
                throw new InvalidOperationException("Id should not be empty");
            }
        }

        /// <summary>
        /// Deactivate an entity
        /// Soft Delete.
        /// </summary>
        /// <returns></returns>
        public Result Deactivate()
        {
            if (!IsDeletable) return Result.Fail("Entity not Deletable");

            Active = false;

            return Result.Ok();
        }

        /// <summary>
        /// Reactivate entity.
        /// </summary>
        public Result Activate()
        {
            Active = true;
            return Result.Ok();
        }


    }
}