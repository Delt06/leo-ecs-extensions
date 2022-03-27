using DELTation.LeoEcsExtensions.Views.Components;
using DELTation.LeoEcsExtensions.Views.Components.Attributes;
using UnityEditor;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Editor.PropertyDrawers
{
    public abstract class ShowIfEntityHasComponentAttributePropertyDrawerBase : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldShow(property))
                EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            ShouldShow(property) ? EditorGUI.GetPropertyHeight(property, label) : 0;

        private bool ShouldShow(SerializedProperty property)
        {
            var componentView = (ComponentView) property.serializedObject.targetObject;
            var showIfAttribute = (ShowIfEntityHasComponentAttributeBase) attribute;
            return componentView.EntityHasComponent() != showIfAttribute.IsInverted;
        }
    }
}