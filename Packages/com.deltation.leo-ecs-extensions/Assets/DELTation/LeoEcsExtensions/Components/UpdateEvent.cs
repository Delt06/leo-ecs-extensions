using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Components
{
    public struct UpdateEvent<T> : IEcsIgnoreInFilter where T : struct
    {
        static UpdateEvent() => UpdateEvents.Register<T>();
    }
}