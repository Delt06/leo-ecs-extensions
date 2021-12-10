using Cube.Components;
using DELTation.LeoEcsExtensions.Utilities;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;

namespace Cube.Simulation
{
    public class CubePositionChangeDebugSystem : IEcsRunSystem
    {
        private readonly EcsFilter _filter;
        private readonly EcsReadOnlyPool<Position> _positions;

        public CubePositionChangeDebugSystem(EcsWorld world)
        {
            _filter = world.Filter<CubeTag>()
                .IncComponentAndUpdateOf<Position>()
                .End();
            _positions = world.GetPool<Position>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var position = ref _positions.Read(i);
                Debug.DrawRay(position.WorldPosition, Vector3.up, Color.red);
            }
        }
    }
}