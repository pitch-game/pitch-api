using System;

namespace Pitch.Squad.API.Exceptions
{
    public class ChemistryException : ApplicationException
    {
        public ChemistryException(string message) : base(message)
        {
        }
    }
}
