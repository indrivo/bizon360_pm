using System;

namespace Gear.CloudStorage.Abstractions.Domain.Models
{
    public class CreateFolderResponseModel
    {
        public CreatedBy CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string ETag { get; set; }

        public string Id { get; set; }

        public LastModifiedBy LastModifiedBy { get; set; }

        public DateTime LastModifiedDateTime { get; set; }

        public string Name { get; set; }

        public ParentReference ParentReference { get; set; }

        public int Size { get; set; }

        public OneDriveFolder Folder { get; set; }
    }

    public class CreatedBy
    {
        public OneDriveUser User { get; set; }
    }

    public class OneDriveUser
    {
        public string DisplayName { get; set; }

        public string Id { get; set; }
    }

    public class LastModifiedBy
    {
        public OneDriveUser User { get; set; }
    }

    public class ParentReference
    {
        public string DriveId { get; set; }

        public string Id { get; set; }

        public string Path { get; set; }
    }

    public class OneDriveFolder
    {
        public int ChildCount { get; set; }
    }
}
