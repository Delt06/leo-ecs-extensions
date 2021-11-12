using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Views.Blueprints
{
    public abstract class EntityBlueprint : ScriptableObject
    {
        protected const string AssetPath = "Entity Blueprint/";
        public abstract void InitializeEntity(EcsEntity entity);
    }
}