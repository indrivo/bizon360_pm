using Gear.Domain.PmEntities.Enums;
using System;

namespace Gear.Domain.PmEntities.Settings
{
    public class ProjectInvoice
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
        public decimal  BudgetMoneyHours { get; set; }

        /// <summary>
        /// Max logged hours per day for team members.
        /// </summary>
        public MaxLogHour MaxLogHoursDay { get; set; } 

        /// <summary>
        /// Max logged hours per activity for team members.
        /// </summary>
        public MaxLogHour MaxLogHoursActivity { get; set; }

        // -----------------------
        // Relationship

        public Project Project { get; set; }

        /// <summary>
        /// Constructor set default values.
        /// </summary>
        public ProjectInvoice()
        {
            Id = Guid.Empty;
            Currency = ProjectCurrency.EUR;
            MaxLogHoursDay = MaxLogHour.NoRestriction;
            MaxLogHoursActivity = MaxLogHour.EightHours;
        }
    }
}
