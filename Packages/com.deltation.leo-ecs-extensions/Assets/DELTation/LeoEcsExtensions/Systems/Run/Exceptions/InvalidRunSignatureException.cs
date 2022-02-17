using System;

namespace DELTation.LeoEcsExtensions.Systems.Run.Exceptions
{
    public class InvalidRunSignatureException : ArgumentException
    {
        public InvalidRunSignatureException(string message, string paramName) : base(message, paramName) { }
    }
}