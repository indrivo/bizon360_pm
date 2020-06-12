using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Gear.Common.Extensions.EnumExtensions
{
    public static class EnumExtensions
    {
        // This extension method is broken out so you can use a similar pattern with 
        // other MetaData elements in the future. This is your base method for each.
        //In short this is generic method to get any type of attribute.
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes.FirstOrDefault();//attributes.Length > 0 ? (T)attributes[0] : null;
        }

        // This method creates a specific call to the above method, requesting the
        // Display MetaData attribute.
        //e.g. [Display(Name = "Sunday")]
        public static string ToDisplayGroup(this Enum value)
        {
            var attribute = value.GetAttribute<DisplayAttribute>();
            return attribute == null ? value.ToString() : attribute.GroupName;
        }

        /// <summary>
        /// This method gets color of type string and returns badge bootstrap class.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string BadgeColorConvert(this string color)
        {
            switch (color)
            {
                case "Blue": return "badge-outline-primary";
                case "Green": return "badge-outline-success";
                case "Red": return "badge-outline-danger";
                case "Cyan": return "badge-outline-info";
                case "Yellow": return "badge-outline-warning";
                case "Purple": return "badge-outline-purple";
                case "Gray": return "badge-outline-secondary";
                default:
                    return "badge-outline-info";
            }
        }
    }
}
