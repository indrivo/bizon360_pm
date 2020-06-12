using System;

namespace Gear.Common.Exceptions
{
    public class BackgroundProcessException : Exception
    {
        public BackgroundProcessException(string exceptionInfo, string processName)
        :base($"Process \"{processName}\" encountered an error \"{exceptionInfo}\"")
        {
        }
    }
}
