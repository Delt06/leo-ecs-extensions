using System;

namespace DELTation.LeoEcsExtensions.Systems
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IgnoreInferredSystemComponentAccessAttribute : Attribute { }
}