using JetBrains.Annotations;

namespace DELTation.LeoEcsExtensions.Systems.Run.Exceptions
{
    internal static class BuiltRunSystemExceptionFactory
    {
        [MustUseReturnValue]
        public static InvalidRunSignatureException NonVoidReturnType(string paramName) =>
            throw new InvalidRunSignatureException("Delegate should have void return type.", paramName);

        [MustUseReturnValue]
        public static InvalidRunSignatureException InParameter(int parameterIndex, string paramName) =>
            throw new InvalidRunSignatureException($"Delegate parameter {parameterIndex} is an in parameter.", paramName
            );

        [MustUseReturnValue]
        public static InvalidRunSignatureException OutParameter(int parameterIndex, string paramName) =>
            throw new InvalidRunSignatureException($"Delegate parameter {parameterIndex} is an out parameter.",
                paramName
            );

        [MustUseReturnValue]
        public static InvalidRunSignatureException RefParameter(int parameterIndex, string paramName) =>
            throw new InvalidRunSignatureException($"Delegate parameter {parameterIndex} is a ref parameter.", paramName
            );
    }
}