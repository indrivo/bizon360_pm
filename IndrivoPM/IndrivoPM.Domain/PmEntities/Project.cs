using System;
using System.Collections.Generic;
using Gear.Common.Entities;
using Gear.Domain.AppEntities;
using Gear.Domain.PmEntities.Enums;
using Gear.Domain.PmEntities.Settings;
using Gear.Domain.PmEntities.Wiki;
using Gear.Domain.PmInterfaces;
using Gear.Sstp.Abstractions.Domain;

namespace Gear.Domain.PmEntities
{
    public class Project : BaseModel, IPmSstp
    {
        public Guid ProjectSettingsId { get; set; }

        public ProjectSettings ProjectSettings { get; set; }

        public Guid ProjectInvoiceId { get; set; }

        public ProjectInvoice ProjectInvoice { get; set; }

        public string Description { get; set; }

        public string ProjectUrl { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal? Budget { get; set; }

        public Guid ProjectGroupId { get; set; }

        public ProjectGroup ProjectGroup { get; set; }

        public Guid? ProjectManagerId { get; set; }

        public ApplicationUser ProjectManager { get; set; }

        public ProjectStatus Status { get; set; } = ProjectStatus.New;

        public ActivityPriority Priority { get; set; } = ActivityPriority.Low;

        public ICollection<ActivityList> ActivityLists { get; set; }

        public ICollection<Sprint> Sprints { get; set; }

        public ICollection<ProjectMember> ProjectMembers { get; set; }

        public ICollection<Activity> Activities { get; set; }

        public ICollection<ChangeRequest> ChangeRequests { get; set; }

        public ICollection<Headline> WikiHeadlines { get; set; }

        public Guid? ServiceTypeId { get; set;}

        public ServiceType ServiceTypes { get; set; }

        public Guid? SolutionTypeId { get; set; }

        public SolutionType SolutionTypes { get; set; }

        public Guid? ProductTypeId { get; set; }

        public ProductType ProductTypes { get; set; }

        public Guid? TechnologyTypeId { get; set; }

        public TechnologyType TechnologyTypes { get; set; }

        public int? Number { get; set; }

        public ICollection<ProjectActivityType> ProjectActivityTypes { get; set; }

        public Project()
        {
            ProjectInvoice = new ProjectInvoice();
            ProjectSettings = new ProjectSettings();
        }
}
}