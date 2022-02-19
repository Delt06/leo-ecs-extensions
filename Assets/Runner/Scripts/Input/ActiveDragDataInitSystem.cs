using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;

namespace Runner.Input
{
    public class ActiveDragDataInitSystem : EcsSystemBase
    {
        [EcsInit]
        private void Init(EcsPool<ActiveDragData> activeDragData, EcsWorld world)
        {
            var entity = world.NewEntity();
            activeDragData.Add(entity);
        }
    }
}