using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DELTation.LeoEcsExtensions.Systems.Run
{
    internal static class ReflectionDelegateFactory
    {
        private static readonly Dictionary<MethodInfo, Type> ActionDelegateTypeCache =
            new Dictionary<MethodInfo, Type>();

        public static Delegate CreateActionDelegate(MethodInfo method, object target)
        {
            if (!ActionDelegateTypeCache.TryGetValue(method, out var delegateType))
            {
                var parameters = method.GetParameters();
                var genericDelegateType = parameters.Length switch
                {
                    0 => typeof(Action),
                    1 => typeof(Action<>),
                    2 => typeof(Action<,>),
                    3 => typeof(Action<,,>),
                    4 => typeof(Action<,,,>),
                    5 => typeof(Action<,,,,>),
                    6 => typeof(Action<,,,,,>),
                    7 => typeof(Action<,,,,,,>),
                    _ => throw new ArgumentOutOfRangeException(),
                };
                delegateType = genericDelegateType.IsGenericTypeDefinition
                    ? genericDelegateType.MakeGenericType(parameters.Select(p => p.ParameterType).ToArray())
                    : genericDelegateType;
                ActionDelegateTypeCache[method] = delegateType;
            }

            return Delegate.CreateDelegate(delegateType, target, method);
        }
    }
}