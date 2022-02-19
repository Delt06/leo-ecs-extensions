using System;
using JetBrains.Annotations;

namespace DELTation.LeoEcsExtensions.ExtendedPools
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class EcsPoolAttribute : Attribute
    {
        public EcsPoolAttribute([NotNull] string createInstanceMethodName, [NotNull] string getComponentTypeMethodName)
        {
            CreateInstanceMethodName =
                createInstanceMethodName ?? throw new ArgumentNullException(nameof(createInstanceMethodName));
            GetComponentTypeMethodName = getComponentTypeMethodName ??
                                         throw new ArgumentNullException(nameof(getComponentTypeMethodName));
        }

        public string CreateInstanceMethodName { get; }
        public string GetComponentTypeMethodName { get; }
    }
}