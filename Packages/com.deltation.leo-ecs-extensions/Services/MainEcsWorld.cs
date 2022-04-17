using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Services
{
    public class MainEcsWorld : IMainEcsWorld
    {
        public MainEcsWorld(EcsWorld world) => World = world;
        public EcsWorld World { get; }
    }
}