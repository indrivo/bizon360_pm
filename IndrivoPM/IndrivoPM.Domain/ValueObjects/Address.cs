using Gear.Common.Extensions.Result;
using Gear.Common.Extensions.ValueObjects;
using System.Collections.Generic;

namespace Gear.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        private string Street { get; }

        private string City { get; }

        private string Country { get; }

        private Address() { }

        protected Address(string street, string city, string country)
        {
            Street = street;
            City = city;
            Country = country;
        }

        public static Result<Address> Create(string street, string city, string country)
        {
            if (string.IsNullOrEmpty(street.Trim()))
                 return Result.Fail<Address>("Street was passed empty or null");

            if (string.IsNullOrEmpty(city.Trim()))
                return Result.Fail<Address>("City was passed empty or null");

            if (string.IsNullOrEmpty(country.Trim()))
                 return Result.Fail<Address>("Country was passed empty or null");


            return Result.Ok(new Address(street, city, country));
        }

        public string GetAddress => $"{Street} {City},{Country}";


        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Street;
            yield return City;
            yield return Country;
        }
    }
}