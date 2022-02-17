using System;
using System.Reflection;
using DELTation.LeoEcsExtensions.Systems.Run.Attributes;
using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Systems.Run
{
    public abstract class EcsRunSystemBase : IEcsRunSystem, IEcsPreInitSystem
    {
        private EcsCallback? _builtRunSystem;

        public void PreInit(EcsSystems systems)
        {
            if (TryFindRunMethod(out var runMethod))
            {
                var world = systems.GetWorld();
                _builtRunSystem = new EcsCallback(world, runMethod, this);
            }

            OnAfterPreInit(systems);
        }

        public void Run(EcsSystems systems)
        {
            _builtRunSystem?.Run();
        }

        private bool TryFindRunMethod(out MethodInfo runMethod)
        {
            var type = GetType();
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var method in methods)
            {
                if (!Attribute.IsDefined(method, typeof(EcsRunAttribute))) continue;
                runMethod = method;
                return true;
            }

            runMethod = default;
            return false;
        }

        protected virtual void OnAfterPreInit(EcsSystems systems) { }
    }
}