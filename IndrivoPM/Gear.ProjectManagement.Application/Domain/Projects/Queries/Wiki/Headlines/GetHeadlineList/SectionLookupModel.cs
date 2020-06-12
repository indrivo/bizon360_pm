using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities.Wiki;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.Wiki.Headlines.GetHeadlineList
{
    public class SectionLookupModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public static Expression<Func<Section, SectionLookupModel>> Projection
        {
            get
            {
                return section => new SectionLookupModel
                {
                    Id = section.Id,
                    Title = section.Name,
                    Content = section.Content
                };
            }
        }

        public static SectionLookupModel Create(Section section)
        {
            return Projection.Compile().Invoke(section);
        }
    }
}
