using DELTation.LeoEcsExtensions.Systems.Run;
using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;

namespace Runner.Input
{
    public class ActiveDragDataInitSystem : EcsSystemBase, IEcsInitSystem
    {
        public void Init(EcsSystems systems)
        {
            World.NewEntityWith<ActiveDragData>();
        }
    }
}