using System;

namespace Pitch.Squad.API.Exceptions
{
    public class InvalidSquadException : ApplicationException
    {
        public InvalidSquadException(string message) : base(message)
        {
        }
    }
}