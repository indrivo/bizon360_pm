using System;

namespace Gear.Identity.Permissions.Domain.Resources
{
    [Flags]
    public enum PaidForModules : long
    {
        None = 0,

        //Access to the CRM module
        CRM = 1,

        //Access to the HRM module
        HRM = 2,

        //Access to the PM module
        PM = 3,

        //Access to the ADM module
        ADM = 4,

        //Enable SSTP module for the platform
        SSTP = 5,

        //Enable Comments module for the platform
        Comments = 6,

        //Enable access to the creation of Custom roles
        Permissions = 7,

        //Enable access to the cloud storage options
        CloudDrive = 8,

        //Enable UI Notifications 
        Notifications = 9,

        //Enable Email notifications
        Emails = 10,

        //Enable Audit
        Audit = 11,

        //Enable access to the error and maintenance module
        Maintenance = 12,

        //Enable access to activities
        Activities = 13
    }
}
