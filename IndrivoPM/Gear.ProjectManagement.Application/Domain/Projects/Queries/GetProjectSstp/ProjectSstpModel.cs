using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectSstp
{
    public class ProjectSstpModel
    {
        public Guid ProjectId { get; set; }

        public Guid? SolutionTypeId { get; set; }

        public string SolutionType { get; set; }

        public Guid? ServiceTypeId { get; set; }

        public string ServiceType { get; set; }

        public Guid? TechnologyTypeId { get; set; }

        public string TechnologyType { get; set; }

        public Guid? ProductTypeId { get; set; }

        public string ProductType { get; set; }

        public static Expression<Func<Project, ProjectSstpModel>> Projection
        {
            get
            {
                return projectSstp => new ProjectSstpModel
                {
                    ProjectId = projectSstp.Id,
                    ProductTypeId = projectSstp.ProductTypeId,
                    ProductType = projectSstp.ProductTypes != null ? projectSstp.ProductTypes.Name : "",
                    ServiceTypeId = projectSstp.ServiceTypeId,
                    ServiceType = projectSstp.ServiceTypes != null ? projectSstp.ServiceTypes.Name : "",
                    SolutionTypeId = projectSstp.SolutionTypeId,
                    SolutionType = projectSstp.ServiceTypes != null ? projectSstp.SolutionTypes.Name : "",
                    TechnologyTypeId = projectSstp.TechnologyTypeId,
                    TechnologyType = projectSstp.TechnologyTypes != null ? projectSstp.TechnologyTypes.Name : ""
                };
            }
        }

        public static ProjectSstpModel Create(Project projectSstp)
        {
            return Projection.Compile().Invoke(projectSstp);
        }
    }
}
