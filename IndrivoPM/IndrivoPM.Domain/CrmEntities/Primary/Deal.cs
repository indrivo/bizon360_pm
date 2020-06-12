using System;
using System.Collections.Generic;
using Gear.Common.Entities;
using Gear.Domain.AppEntities;
using Gear.Domain.CrmEntities.Enums;
using Gear.Domain.CrmEntities.ManyToManyEntities;
using Gear.Domain.CrmEntities.Secondary;

namespace Gear.Domain.CrmEntities.Primary
{
    public class Deal : BaseModel
    {
        #region Fields

        public decimal DealValue { get; set; }

        public DealStatus DealStatus { get; set; }

        public string Description { get; set; }

        public string Region { get; set; }

        public DateTime? DayTimeOfContract { get; set; }

        public DateTime? OfferProposalDeadLine { get; set; }

        public DateTime? DetailClarificationDeadLine { get; set; }

        #endregion

        #region Fk

        public Guid DealStageId { get; set; }

        public Guid OrganizationId { get; set; }

        public Guid ApplicationUserId { get; set; }

        public Guid CurrencyId { get; set; }

        public Guid? DealProductTypeId { get; set; }

        public Guid? DealServiceTypeId { get; set; }

        public Guid? DealSolutionTypeId { get; set; }

        public Guid? DealTechnologyTypeId { get; set; }

        public Guid? DealSourceId { get; set; }

        public Guid? LostCauseId { get; set; }

        public Guid? NoGoCauseId { get; set; }

        #endregion

        #region BindingToModels

        public DealStage DealStage { get; set; }

        public ClientOrganization ClientOrganization { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public Currency Currency { get; set; }

        public Source DealSource { get; set; }

        public LostCause LostCause { get; set; }

        public NoGoCause NoGoCause { get; set; }

        #endregion

        #region One-To-One-To-Many

        public ICollection<DealContact> Contacts { get; set; }

        public ICollection<DealTagMates> DealTagMates { get; set; }

        #endregion
    }
}