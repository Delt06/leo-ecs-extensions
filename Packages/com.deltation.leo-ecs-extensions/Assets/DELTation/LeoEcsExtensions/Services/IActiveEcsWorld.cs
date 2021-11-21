#if LEOECS_EXTENSIONS_LITE
using EcsWorld = Leopotam.EcsLite.EcsWorld;

#else
using EcsWorld = Leopotam.Ecs.EcsWorld;
#endif

namespace DELTation.LeoEcsExtensions.Services
{
    public interface IActiveEcsWorld
    {
        EcsWorld World { get; }
    }
}