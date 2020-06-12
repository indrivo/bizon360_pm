using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bizon360.Utils
{
    public static class UiExtensions
    {
        public static HtmlString GetRemainingTime(this DateTime? date)
        {
            if (!date.HasValue) return new HtmlString(string.Empty);

            var tagBuilder = new TagBuilder("span");
            var remainingTime = (int) (date.Value - DateTime.Now).TotalDays / 7;
            var type = "weeks";

            if (remainingTime == 0)
            {
                tagBuilder.AddCssClass("color-danger");

                if (date.Value.Year - DateTime.Now.Year < 0)
                {
                    remainingTime = (DateTime.DaysInMonth(date.Value.Year, date.Value.Month) - date.Value.Day + DateTime.Now.Day ) * -1;
                }
                else
                {
                    remainingTime = date.Value.Day - DateTime.Now.Day;
                }

                type = "days";
            }
            else if (remainingTime <= 4)
            {
                tagBuilder.AddCssClass("color-danger");
            }

            tagBuilder.InnerHtml.Append(remainingTime < 0
                ? $"overdue by {remainingTime * -1} {type}."
                : $"{remainingTime} {type} remaining.");

            var sb = new StringBuilder();
            using (var writer = new StringWriter())
            {
                tagBuilder.WriteTo(writer, HtmlEncoder.Default);
                sb.Append(writer);

                return new HtmlString(sb.ToString());
            }

        }
    }
}
