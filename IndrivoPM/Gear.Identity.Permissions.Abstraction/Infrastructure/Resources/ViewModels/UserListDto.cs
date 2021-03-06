﻿using System;

namespace Gear.Identity.Permissions.Infrastructure.Resources.ViewModels
{
    public class UserListDto
    {
        public UserListDto(string email, string roleNames, string moduleNames)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            RoleNames = roleNames ?? throw new ArgumentNullException(nameof(roleNames));
            ModuleNames = moduleNames ?? throw new ArgumentNullException(nameof(moduleNames));
        }

        public string Email { get; set; }

        public string RoleNames { get; set; }

        public string ModuleNames { get; set; }


    }
}
