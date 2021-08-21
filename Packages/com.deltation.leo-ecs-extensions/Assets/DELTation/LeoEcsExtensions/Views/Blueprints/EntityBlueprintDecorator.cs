using Leopotam.Ecs;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Views.Blueprints
{
    public abstract class EntityBlueprintDecorator : EntityBlueprint
    {
        [SerializeField] private EntityBlueprint _baseBlueprint = default;

        public sealed override void InitializeEntity(EcsEntity entity)
        {
            _baseBlueprint.InitializeEntity(entity);
            InitializeEntityAfterBase(entity);
        }

        protected abstract void InitializeEntityAfterBase(EcsEntity entity);
    }
}