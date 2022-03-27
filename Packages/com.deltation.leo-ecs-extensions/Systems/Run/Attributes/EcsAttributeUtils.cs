using System;

namespace DELTation.LeoEcsExtensions.Systems.Run.Attributes
{
    internal static class EcsAttributeUtils
    {
        public static void ValidateComponentType(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!type.IsValueType)
                throw new ArgumentException("Type is required to be a value type.", nameof(type));
        }
    }
}