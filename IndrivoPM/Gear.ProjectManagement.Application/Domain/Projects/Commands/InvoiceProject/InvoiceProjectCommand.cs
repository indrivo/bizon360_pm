using System;
using System.ComponentModel;
using Gear.Domain.PmEntities.Enums;
using MediatR;

namespace Gear.ProjectManagement.Manager.Domain.Projects.Commands.InvoiceProject
{
    public class InvoiceProjectCommand : IRequest
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        [DisplayName("Currency")]
        public ProjectCurrency Currency { get; set; }

        [DisplayName("Budget in money")]
        public decimal BudgetMoney { get; set; }

        [DisplayName("Default rate/hour")]
        public decimal HourRateMoney { get; set; }

        [DisplayName("Budget in hours")]
        public decimal BudgetMoneyHours { get; set; }

        [DisplayName("Log. hours/day")]
        public MaxLogHour MaxLogHoursDay { get; set; }

        [DisplayName("Log. hours/activity")]
        public MaxLogHour MaxLogHoursActivity { get; set; }

/*        [DisplayName("Solution type")]
        public Guid? SolutionTypeId { get; set; }

        [DisplayName("Service type")]
        public Guid? ServiceTypeId { get; set; }

        [DisplayName("Product type")]
        public Guid? ProductTypeId { get; set; }

        [DisplayName("Technology type")]
        public Guid? TechnologyTypeId { get; set; }*/
    }
}
