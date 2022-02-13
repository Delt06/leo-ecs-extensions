using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Tests.EditMode
{
    internal static class EcsTestUtils
    {
        public static void PreventAutomaticDeletion(EcsWorld world, int entity)
        {
            world.GetPool<PrivateComponent>().Add(entity);
        }

        private struct PrivateComponent { }
    }
}