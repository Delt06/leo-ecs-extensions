using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Views.Blueprints
{
    public abstract class EntityBlueprint : ScriptableObject
    {
        public abstract void InitializeEntity(EcsEntity entity);

        protected const string AssetPath = "Entity Blueprint/";
    }
}