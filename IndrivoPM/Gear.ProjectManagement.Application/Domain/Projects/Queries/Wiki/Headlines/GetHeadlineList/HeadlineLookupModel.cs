using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gear.Domain.PmEntities.Wiki;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineList
{
    public class HeadlineLookupModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public ICollection<SectionLookupModel> Sections { get; set; }

        public static Expression<Func<Headline, HeadlineLookupModel>> Projection
        {
            get
            {
                return headline => new HeadlineLookupModel
                {
                    Id = headline.Id,
                    Title = headline.Name,
                    Sections = headline.Sections.OrderBy(s => s.CreatedTime).Select(SectionLookupModel.Create).ToList()
                };
            }
        }

        public static HeadlineLookupModel Create(Headline headline)
        {
            return Projection.Compile().Invoke(headline);
        }
    }
}
