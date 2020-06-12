using System;

namespace Gear.Sstp.Abstractions.Exceptions
{
    public class IdNullOrEmptyException : Exception
    {
        public IdNullOrEmptyException() : base("The Id you provided was null")
        {
        }
    }
}
