#if UNITY_EDITOR

using JetBrains.Annotations;
using Leopotam.EcsLite;

// ReSharper disable InconsistentNaming
// ReSharper disable MustUseReturnValue
// ReSharper disable UnusedVariable

namespace DELTation.LeoEcsExtensions.Systems.Run
{
    public static class RunSystemsSourceTemplates
    {
        [SourceTemplate]
        public static void mapTo(this EcsWorld.Mask filterMask)
        {
            filterMask.MapTo((EcsFilter filter) =>
                {
                    foreach (var i in filter)
                    {
                        //$ $END$
                    }
                }
            );
        }
    }
}

#endif