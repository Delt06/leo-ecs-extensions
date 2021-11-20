using Leopotam.Ecs;

namespace DELTation.LeoEcsExtensions.Components
{
    public struct UpdateEvent<T> : IEcsIgnoreInFilter, IEcsAutoReset<UpdateEvent<T>> where T : struct
    {
#pragma warning disable 0414
        // ReSharper disable once StaticMemberInGenericType
        private static bool _initialized;
#pragma warning restore 0414

        static UpdateEvent() => UpdateEvents.Register<T>();

        public void AutoReset(ref UpdateEvent<T> c)
        {
            // Ensure static constructor is called
            _initialized = true;
        }
    }
}