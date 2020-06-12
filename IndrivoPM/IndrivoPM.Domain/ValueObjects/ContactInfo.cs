using System.Collections.Generic;
using System.Text.RegularExpressions;
using Gear.Common.Extensions.Result;
using Gear.Common.Extensions.ValueObjects;

namespace Gear.Domain.ValueObjects
{
    public class ContactInfo : ValueObject
    {
        public string PhoneNumber { get; private set; }

        public string Email { get; private set; }

        private ContactInfo() { }

        protected ContactInfo(string phoneNumber, string email)
        {
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public static Result<ContactInfo> Create(string phoneNumber, string email)
        {
            email = (email ?? string.Empty).Trim();

            if (string.IsNullOrEmpty(email.Trim()))
                return Result.Fail<ContactInfo>("Email should not be empty");

            if (!Regex.IsMatch(email, @"^(.+)@(.+)$"))
                return Result.Fail<ContactInfo>("Email is invalid");

            if(string.IsNullOrEmpty(phoneNumber.Trim()))
                return Result.Fail<ContactInfo>("Phone number should not be empty");

            if (!Regex.IsMatch(email, @"^[+] *[(]{ 0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$"))
                return Result.Fail<ContactInfo>("Phone number is invalid");


            return Result.Ok(new ContactInfo(phoneNumber,email));
        }


        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return PhoneNumber;
            yield return Email;
        }
    }
}
