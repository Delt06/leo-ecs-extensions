#if LEOECS_EXTENSIONS_LITE
using Leopotam.EcsLite;

#else
using Leopotam.Ecs;
#endif

namespace DELTation.LeoEcsExtensions.Components
{
    public struct UpdateEvent<T> :
        IEcsAutoReset<UpdateEvent<T>>
#if !LEOECS_EXTENSIONS_LITE
        , IEcsIgnoreInFilter
#endif
        where T : struct
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