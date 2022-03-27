using UnityEngine;

namespace DELTation.LeoEcsExtensions.Views.Components.Attributes
{
    internal abstract class ShowIfEntityHasComponentAttributeBase : PropertyAttribute
    {
        public bool IsInverted { get; protected set; }
    }
}