using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Composition.Di
{
    internal sealed class EcsEntryPointView : MonoBehaviour
    {
        private EcsEntryPoint _ecsEntryPoint;

        public void Construct(EcsEntryPoint ecsEntryPoint)
        {
            _ecsEntryPoint = ecsEntryPoint;
        }

        private void Start()
        {
            _ecsEntryPoint.Start();
        }

        private void Update()
        {
            _ecsEntryPoint.Update();
        }

        private void FixedUpdate()
        {
            _ecsEntryPoint.FixedUpdate();
        }

        private void LateUpdate()
        {
            _ecsEntryPoint.LateUpdate();
        }

        private void OnDestroy()
        {
            _ecsEntryPoint.OnDestroy();
        }

#if UNITY_EDITOR
        [CanBeNull]
        public EcsSystems Systems => _ecsEntryPoint?.Systems;

        [CanBeNull]
        public EcsSystems PhysicsSystems => _ecsEntryPoint?.PhysicsSystems;

        [CanBeNull]
        public EcsSystems LateSystems => _ecsEntryPoint?.LateSystems;
#endif
    }
}