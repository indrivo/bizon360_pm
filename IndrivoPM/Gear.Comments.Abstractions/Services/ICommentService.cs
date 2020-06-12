using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gear.Comments.Abstractions.Services
{
    public interface ICommentService<TComment> where TComment:class
    {
        Task Create(TComment entity);
    }
}
