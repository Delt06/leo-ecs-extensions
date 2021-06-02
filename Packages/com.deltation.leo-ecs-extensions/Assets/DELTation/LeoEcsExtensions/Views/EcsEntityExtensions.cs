using DELTation.LeoEcsExtensions.Components;
using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Views
{
    public static class EcsEntityExtensions
    {
        public static EcsEntity ReplaceViewBackRef<T>(this EcsEntity entity, T view) where T : IEntityView =>
            entity.Replace(new ViewBackRef<T> { View = view });

        public static EcsEntity ReplaceUnityObjectDataData<T>(this EcsEntity entity, T @object) where T : Object =>
            entity.Replace(new UnityObjectData<T> { Object = @object });
    }
}