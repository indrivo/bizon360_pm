using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Gear.Manager.Core.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public Dictionary<string, string[]> Failures { get; set; }

        public ValidationException() : base("one or more validation failures have occured")
        {
            Failures = new Dictionary<string, string[]>();
        }

        public ValidationException(List<ValidationFailure> failures) : this()
        {
            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                Failures.Add(propertyName, propertyFailures);
            }

        }
    }
}
