using Leopotam.EcsLite;

namespace DELTation.LeoEcsExtensions.Composition
{
    public interface ISystemBuilder
    {
        void Populate(EcsSystems systems);
    }
}