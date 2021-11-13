using DELTation.LeoEcsExtensions.Views.Components;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Editor
{
    [CustomEditor(typeof(ComponentView), true)]
    public class ComponentViewEditor : UnityEditor.Editor
    {
        private ComponentView _componentView;
        private Color _oldBgColor;

        public override void OnInspectorGUI()
        {
            _componentView = (ComponentView) target;
            base.OnInspectorGUI();

            DrawEntity();
            DrawComponentButtons();
        }

        private void DrawEntity()
        {
            if (!_componentView.Entity.IsAlive()) return;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Entity", _componentView.Entity.ToString());
            EditorGUI.EndDisabledGroup();

            var labelStyle = new GUIStyle(GUI.skin.label)
            {
                richText = true,
            };

            var componentStatus = _componentView.EntityHasComponent()
                ? "<color=green>added</color>"
                : "<color=red>not added</color>";
            EditorGUILayout.LabelField($"Component: {componentStatus}.", labelStyle);
        }

        private void DrawComponentButtons()
        {
            if (!_componentView.Entity.IsAlive()) return;


            GUILayout.BeginHorizontal();
            RecordBackgroundColor();

            SetBackgroundColor(new Color(0.75f, 0.25f, 0.25f));

            EditorGUI.BeginDisabledGroup(!_componentView.EntityHasComponent());
            if (GUILayout.Button("Delete component"))
                _componentView.TryDeleteComponent();
            EditorGUI.EndDisabledGroup();

            SetBackgroundColor(new Color(0.25f, 0.75f, 0.25f));

            EditorGUI.BeginDisabledGroup(_componentView.EntityHasComponent());
            if (GUILayout.Button("Add component"))
                _componentView.TryAddComponent();
            EditorGUI.EndDisabledGroup();

            RestoreBackgroundColor();
            GUILayout.EndHorizontal();
        }

        private static void SetBackgroundColor(Color backgroundColor)
        {
            GUI.backgroundColor = backgroundColor;
        }

        private void RecordBackgroundColor()
        {
            _oldBgColor = GUI.backgroundColor;
        }

        private void RestoreBackgroundColor()
        {
            GUI.backgroundColor = _oldBgColor;
        }
    }
}