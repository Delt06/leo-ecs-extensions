using System;
using JetBrains.Annotations;

namespace DELTation.LeoEcsExtensions.Systems.Run.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class EcsExcAttribute : Attribute
    {
        public EcsExcAttribute([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            ValidateType(type);
            Type = type;
        }

        public Type Type { get; }

        private static void ValidateType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!type.IsValueType)
                throw new ArgumentException("Type is required to be a value type.", nameof(type));
        }
    }
}