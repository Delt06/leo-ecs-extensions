using System;

namespace DELTation.LeoEcsExtensions.Systems.Run.Exceptions
{
    public class InvalidRunSignatureException : ArgumentException
    {
        internal InvalidRunSignatureException(string message, string paramName) : base(message, paramName) { }
        internal InvalidRunSignatureException(string message) : base(message) { }
    }
}