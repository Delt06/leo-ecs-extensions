using System;
using JetBrains.Annotations;

namespace DELTation.LeoEcsExtensions.Systems
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SystemComponentAccessAttribute : Attribute
    {
        public readonly ComponentAccessType AccessType;
        public readonly Type Type;

        [UsedImplicitly]
        public SystemComponentAccessAttribute([NotNull] Type type, ComponentAccessType accessType)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            AccessType = accessType;
        }
    }
}