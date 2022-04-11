using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Systems
{
    /// <summary>
    ///     System for removing OneFrame component.
    ///     Same as OneFrame system from LeoECS core.
    /// </summary>
    /// <typeparam name="T">OneFrame component type.</typeparam>
    internal sealed class RemoveOneFrame<T> : IEcsRunSystem, IEcsInitSystem
        where T : struct

    {
        private EcsFilter _filter;
        private EcsPool<T> _pool;

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
    }
}