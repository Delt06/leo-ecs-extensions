#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;

#else
using Leopotam.Ecs;
#endif

namespace DELTation.LeoEcsExtensions.Systems
{
    /// <summary>
    ///     System for removing OneFrame component.
    ///     Same as OneFrame system from LeoECS core.
    /// </summary>
    /// <typeparam name="T">OneFrame component type.</typeparam>
    internal sealed class RemoveOneFrame<T> : IEcsRunSystem
#if LEOECS_EXTENSIONS_LITE
        , IEcsInitSystem
#endif
        where T : struct

    {
#if LEOECS_EXTENSIONS_LITE
        private EcsPool<T> _pool;
        private EcsFilter _filter;

        public void Init(EcsSystems systems)
        {
            var world = systems.GetWorld();
            _pool = world.GetPool<T>();
            _filter = world.Filter<T>().End();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                _pool.Del(i);
            }
        }

#else
        private readonly EcsFilter<T> _oneFrames = null;

        void IEcsRunSystem.Run()
        {
            for (var idx = _oneFrames.GetEntitiesCount() - 1; idx >= 0; idx--)
            {
                _oneFrames.GetEntity(idx).Del<T>();
            }
        }
#endif
    }
}