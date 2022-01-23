using DELTation.LeoEcsExtensions.Views.Components.Attributes;
using UnityEditor;

namespace DELTation.LeoEcsExtensions.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(ShowIfEntityHasComponentAttribute))]
    public class
        ShowIfEntityHasComponentAttributePropertyDrawer : ShowIfEntityHasComponentAttributePropertyDrawerBase { }
}