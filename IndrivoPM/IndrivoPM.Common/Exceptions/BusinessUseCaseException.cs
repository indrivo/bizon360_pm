using System;
using System.Collections.Generic;
using System.Linq;

namespace Gear.Common.Exceptions
{
    public class BusinessUseCaseException : Exception
    {
        //TODO: Think over needing to remove the list to simple string
        public BusinessUseCaseException(IList<string> violations) 
            : base($"Violation of following use case rules " +
                   $"{(violations.Any() ? violations.Distinct().Aggregate((a, b) => a + "; " + b) : "")}")
        {
            
        }
    }
}
