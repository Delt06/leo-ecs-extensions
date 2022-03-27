using System;
using JetBrains.Annotations;

namespace DELTation.LeoEcsExtensions.Systems.Run.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class EcsIncAttribute : Attribute
    {
        public EcsIncAttribute([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            EcsAttributeUtils.ValidateComponentType(type);
            Type = type;
        }

        public Type Type { get; }
    }
}