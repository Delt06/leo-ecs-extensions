using DELTation.LeoEcsExtensions.Views.Components.Attributes;
using UnityEditor;

namespace DELTation.LeoEcsExtensions.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(HideIfEntityHasComponentAttribute))]
    public class
        HideIfEntityHasComponentAttributePropertyDrawer : ShowIfEntityHasComponentAttributePropertyDrawerBase { }
}