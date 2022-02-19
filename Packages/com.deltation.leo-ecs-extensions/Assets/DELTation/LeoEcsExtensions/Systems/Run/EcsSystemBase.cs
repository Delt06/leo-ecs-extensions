using System;
using System.Reflection;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Systems.Run
{
    public abstract class EcsSystemBase : IEcsRunSystem, IEcsPreInitSystem, IEcsInitSystem
    {
        private EcsCallback? _builtInitSystem;
        private EcsCallback? _builtRunSystem;

        public void Init(EcsSystems systems)
        {
            _builtInitSystem?.Run();
        }

        public void PreInit(EcsSystems systems)
        {
            TryInitializeBuiltSystem(systems, typeof(EcsInitAttribute), ref _builtInitSystem);
            TryInitializeBuiltSystem(systems, typeof(EcsRunAttribute), ref _builtRunSystem);

            OnAfterPreInit(systems);
        }

        public void Run(EcsSystems systems)
        {
            _builtRunSystem?.Run();
        }

        private void TryInitializeBuiltSystem(EcsSystems systems, Type attributeType, ref EcsCallback? builtSystem)
        {
            if (!TryFindMethod(attributeType, out var method)) return;
            var world = systems.GetWorld();
            builtSystem = new EcsCallback(world, method, this);
        }

        private bool TryFindMethod(Type attribute, out MethodInfo runMethod)
        {
            var type = GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, attribute)) continue;
                runMethod = method;
                return true;
            }

            runMethod = default;
            return false;
        }

        protected virtual void OnAfterPreInit(EcsSystems systems) { }
    }
}