using System.Threading.Tasks;
using Gear.CloudStorage.Abstractions.Domain;
using Gear.Manager.Core.EntityServices.CloudStorage.Commands.MoveFolderContentIntoAnotherDirectory;
using Gear.OneDriveCloudStorage.Service;
using Xunit;

namespace OneDriveCloudStorage.IntegrationTests
{
    public class MoveFolderContentTests
    {
        public MoveFolderContentTests()
        {

        }

        [Fact]
        public async Task MoveFolderContentIntoAnotherDirectory_ValidData_True()
        {
            var command = new MoveFolderContentIntoAnotherDirectoryCommand
            {
                ExternalProvider = ExternalProviders.OneDrive,
                FolderName = "Tmp",
                NewPath = @"drive/special/approot:/PM/Projects/P0/A0",
                OldPath = @"drive/special/approot:/PM/Projects/Bizon360 Platform762df025-a069-4427-b764-703b26d11d2c/Files bug fixing2a46cc75-50b3-494b-b91e-6263d84f8c94",
                ProjectNumber = 0
            };
        }
    }
}
