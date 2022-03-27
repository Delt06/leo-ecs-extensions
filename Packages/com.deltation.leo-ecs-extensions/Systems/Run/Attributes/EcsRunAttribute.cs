using System;
using UnityEngine.Scripting;

namespace DELTation.LeoEcsExtensions.Systems.Run.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class EcsRunAttribute : PreserveAttribute { }
}