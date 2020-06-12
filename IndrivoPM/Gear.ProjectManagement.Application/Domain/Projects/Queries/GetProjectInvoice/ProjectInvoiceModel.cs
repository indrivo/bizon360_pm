using System;
using System.Linq.Expressions;
using Gear.Domain.PmEntities.Enums;
using Gear.Domain.PmEntities.Settings;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Queries.GetProjectInvoice
{
    public class ProjectInvoiceModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Project id to apply the invoice.
        /// </summary>
        public Guid ProjectId { get; set; }

        /// <summary>
        /// Type of currency money.
        /// </summary>
        public ProjectCurrency Currency { get; set; }

        /// <summary>
        /// Budget in money for project.
        /// </summary>
        public decimal BudgetMoney { get; set; }

        /// <summary>
        /// Rate money team per hour.
        /// </summary>
        public decimal HourRateMoney { get; set; }

        /// <summary>
        /// Budget in money per hour for project.
        /// </summary>
        public decimal BudgetMoneyHours { get; set; }

        /// <summary>
        /// Max logged hours per day for team members.
        /// </summary>
        public MaxLogHour MaxLogHoursDay { get; set; }

        /// <summary>
        /// Max logged hours per activity for team members.
        /// </summary>
        public MaxLogHour MaxLogHoursActivity { get; set; }

        public Guid? SolutionTypeId { get; set; }

        public Guid? ServiceTypeId { get; set; }

        public Guid? ProductTypeId { get; set; }

        public Guid? TechnologyTypeId { get; set; }

        public static Expression<Func<ProjectInvoice, ProjectInvoiceModel>> Projection
        {
            get
            {
                return invoice => new ProjectInvoiceModel
                {
                    Id = invoice.Id,
                    ProjectId = invoice.Id,
                    BudgetMoney = invoice.BudgetMoney,
                    BudgetMoneyHours = invoice.BudgetMoneyHours,
                    MaxLogHoursActivity = invoice.MaxLogHoursActivity,
                    MaxLogHoursDay = invoice.MaxLogHoursDay,
                    HourRateMoney = invoice.HourRateMoney,
                    Currency = invoice.Currency,

                };
            }
        }

        public static ProjectInvoiceModel Create(ProjectInvoice invoice)
        {
            return Projection.Compile().Invoke(invoice);
        }
    }
}
