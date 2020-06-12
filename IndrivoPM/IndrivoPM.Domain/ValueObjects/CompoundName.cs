using Gear.Common.Extensions.Result;
using Gear.Common.Extensions.ValueObjects;
using System.Collections.Generic;

namespace Gear.Domain.ValueObjects
{
    public class CompoundName : ValueObject
    {
        public string FirstName { get; }

        public string LastName { get; }

        private CompoundName() { }

        protected CompoundName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public static Result<CompoundName> Create(string firstName,string lastName)
        {
            if (string.IsNullOrEmpty(firstName.Trim()))
                return Result.Fail<CompoundName>("First Name was passed empty or null");

            if (string.IsNullOrEmpty(lastName.Trim()))
                return Result.Fail<CompoundName>("Last Name was passed empty or null");

            return Result.Ok(new CompoundName(firstName, lastName));
        }


        /// <summary>
        /// Override required for the entity comparison
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        public string GetFullName => $"{FirstName} {LastName}";


        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return FirstName;
            yield return LastName;
        }
    }
}
