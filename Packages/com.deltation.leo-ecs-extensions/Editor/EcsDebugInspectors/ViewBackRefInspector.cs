using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Views;
using JetBrains.Annotations;
using Leopotam.EcsLite.UnityEditor;
using UnityEditor;

namespace DELTation.LeoEcsExtensions.Editor.EcsDebugInspectors
{
    [UsedImplicitly]
    public class ViewBackRefInspector : EcsComponentInspectorTyped<ViewBackRef>
    {
        public override bool OnGuiTyped(string label, ref ViewBackRef value, EcsEntityDebugView entityView)
        {
            EditorGUILayout.LabelField(label);

            const string viewLabel = "View";
            if (value.View is EntityView || value.View == null)
            {
                var viewObject = value.View as EntityView;
                var newValue = EditorGUILayout.ObjectField(viewLabel, viewObject, typeof(EntityView), true);
                if (ReferenceEquals(value.View, newValue))
                    return false;

                value.View = (IEntityView) newValue;
                return true;
            }

            EditorGUILayout.LabelField(viewLabel, value.View?.ToString());
            return false;
        }
    }
}