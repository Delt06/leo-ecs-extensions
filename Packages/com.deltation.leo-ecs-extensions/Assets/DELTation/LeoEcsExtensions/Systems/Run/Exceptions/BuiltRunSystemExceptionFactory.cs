using System;
using JetBrains.Annotations;

namespace DELTation.LeoEcsExtensions.Systems.Run.Exceptions
{
    internal static class BuiltRunSystemExceptionFactory
    {
        [MustUseReturnValue]
        public static InvalidRunSignatureException NonVoidReturnType(string paramName) =>
            new InvalidRunSignatureException("Delegate should have void return type.", paramName);

        [MustUseReturnValue]
        public static InvalidRunSignatureException InParameter(int parameterIndex, string paramName) =>
            new InvalidRunSignatureException($"Delegate parameter {parameterIndex} is an in parameter.", paramName
            );

        [MustUseReturnValue]
        public static InvalidRunSignatureException OutParameter(int parameterIndex, string paramName) =>
            new InvalidRunSignatureException($"Delegate parameter {parameterIndex} is an out parameter.",
                paramName
            );

        [MustUseReturnValue]
        public static InvalidRunSignatureException RefParameter(int parameterIndex, string paramName) =>
            new InvalidRunSignatureException($"Delegate parameter {parameterIndex} is a ref parameter.", paramName
            );

        [MustUseReturnValue]
        public static InvalidRunSignatureException InvalidParameterType(Type parameterType) =>
            throw new InvalidRunSignatureException(
                $"Parameter type {parameterType} cannot be passed in built run systems."
            );

        [MustUseReturnValue]
        public static InvalidRunSignatureException NoIncludes() =>
            new InvalidRunSignatureException(
                "Signature contains a filter, it does not have any pools for includes."
            );
    }
}