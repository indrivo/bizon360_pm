using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Gear.Identity.Permissions.Infrastructure.Attributes;

namespace Gear.Identity.Permissions.Infrastructure.Resources.ViewModels
{
    public class PermissionDisplay
    {
        /// <summary>
        /// Constructor used with validations
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="permission"></param>
        /// <param name="moduleName"></param>
        public PermissionDisplay(string groupName, string name, string description, Domain.Resources.Permissions permission,
            string moduleName)
        {
            Permission = permission;
            GroupName = groupName;
            ShortName = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            ModuleName = moduleName;
        }

        /// <summary>
        /// GroupName, which groups permissions working in the same area
        /// </summary>
        public string GroupName { get; private set; }


        /// <summary>
        /// ShortName of the permission - often says what it does, e.g. Read
        /// </summary>
        public string ShortName { get; private set; }


        /// <summary>
        /// Long description of what action this permission allows 
        /// </summary>
        public string Description { get; private set; }


        /// <summary>
        /// Gives the actual permission
        /// </summary>
        public Domain.Resources.Permissions Permission { get; private set; }


        /// <summary>
        /// Contains an optional paidForModule that this feature is linked to
        /// </summary>
        public string ModuleName { get; private set; }


        /// <summary>
        /// This returns the information
        /// </summary>
        /// <returns></returns>
        public static List<PermissionDisplay> GetPermissionsToDisplay(Type enumType)
        {
            var result = new List<PermissionDisplay>();
            foreach (var permissionName in Enum.GetNames(enumType))
            {
                var member = enumType.GetMember(permissionName);

                //This allows you to obsolete a permission and it won't be shown as a possible option, but is still there so you won't reuse the number
                var obsoleteAttribute = member[0].GetCustomAttribute<ObsoleteAttribute>();
                if (obsoleteAttribute != null)
                    continue;


                //If there is no DisplayAttribute then the Enum is not used
                var displayAttribute = member[0].GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute == null)
                    continue;

                //Gets the optional PaidForModule that a permission can be linked to
                var moduleAttribute = member[0].GetCustomAttribute<LinkedToModuleAttribute>();

                var permission = (Domain.Resources.Permissions)Enum.Parse(enumType, permissionName, false);

                result.Add(new PermissionDisplay(displayAttribute.GroupName, displayAttribute.Name,
                        displayAttribute.Description, permission, moduleAttribute?.PaidForModule.ToString()));
            }

            return result;
        }
    }
}
