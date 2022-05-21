using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Systems
{
    /// <summary>
    ///     System for removing entities with OneFrame components.
    /// </summary>
    /// <typeparam name="T">OneFrame component type.</typeparam>
    internal sealed class RemoveOneFrameEntity<T> : IEcsRunSystem, IEcsInitSystem
        where T : struct

    {
        private EcsFilter _filter;
        private EcsWorld _world;

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<T>().End();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                _world.DelEntity(i);
            }
        }
    }
}