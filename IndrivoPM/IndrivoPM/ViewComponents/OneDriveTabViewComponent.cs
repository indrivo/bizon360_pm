using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Word;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.Common.Extensions.Result;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.RefreshUserToken;
using Gear.Manager.Core.EntityServices.CloudStorage.Querries.GetChildren;
using Gear.Manager.Core.Infrastructure.MediatorResultHandler;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bizon360.ViewComponents
{
    public class OneDriveTabViewComponent : ViewComponent
    {
        private readonly IMediatrResultFactory _mediatorFactory;
        private readonly IMediator _mediatR;

        public OneDriveTabViewComponent(IMediator mediatR, IMediatrResultFactory mediatrResultFactory)
        {
            _mediatR = mediatR;
            _mediatorFactory = mediatrResultFactory;
        }

        /// <summary>
        /// Invoke ViewComponent
        /// </summary>
        /// <returns></returns>
        public async Task<IViewComponentResult> InvokeAsync(OneDriveTabDto dto)
        {
            Result refreshUserToken = await _mediatorFactory.Execute(new RefreshCurrentUserTokenCommand() { ExternalProvider = ExternalProviders.OneDrive });

            if (refreshUserToken.IsSuccess)
            {
                var model = new OneDriveListViewModel()
                {
                    Paths = dto.Paths,
                    FolderName = dto.FolderName,
                    MetaData = new List<CloudMetaData>()
                };
                try
                {
                    model.MetaData = await _mediatR.Send(new GetChildrenQuery()
                    { ExternalProvider = dto.ExternalProvider, FilePath = dto.Paths[0] });
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Catch this" + e.Message);
                }

                return View("OneDriveTab", model);
            }

            throw new InvalidOperationException("Catch this");

        }
    }

    public class OneDriveTabDto
    {
        public IList<string> Paths { get; set; }

        public string FolderName { get; set; }

        public ExternalProviders ExternalProvider { get; set; } = ExternalProviders.OneDrive;

        public string FileName { get; set; }

    }


    public class OneDriveListViewModel
    {
        public List<CloudMetaData> MetaData { get; set; }

        public IList<string> Paths { get; set; }

        public string FolderName { get; set; }
    }
}
