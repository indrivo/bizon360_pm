using System.Collections.Generic;

namespace Gear.ProjectManagement.Manager.Domain.MainComments.Queries.GetMainCommentList
{
    public class MainCommentListViewModel
    {
        public IList<MainCommentLookupModel> Comments { get; set; }
    }
}
