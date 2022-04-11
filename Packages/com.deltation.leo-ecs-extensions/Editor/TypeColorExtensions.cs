using System;
using JetBrains.Annotations;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Editor
{
    internal static class TypeColorExtensions
    {
        public static Color GetColor([NotNull] this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            var hashCode = unchecked((uint) type.GetHashCode());

            const int divider = 7;
            var hue = hashCode % divider / (divider - 1f);
            const float saturation = 1f;
            const float value = 1f;
            return Color.HSVToRGB(hue, saturation, value);
        }
    }
}