using System;

namespace DELTation.LeoEcsExtensions.Systems.Run.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class EcsIgnoreIncAttribute : Attribute { }
}