using DELTation.LeoEcsExtensions.Compatibility;
using DELTation.LeoEcsExtensions.Views.Components;
using UnityEditor;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
#endif

namespace DELTation.LeoEcsExtensions.Editor
{
    [CustomEditor(typeof(ComponentView), true)]
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class ComponentViewEditor :
#if ODIN_INSPECTOR
        OdinEditor
#else
        UnityEditor.Editor
#endif
    {
        private Color _oldBgColor;

        protected virtual void OnEnable()
        {
            Update();
            EditorApplication.update += Update;
        }

        protected virtual void OnDisable()
        {
            EditorApplication.update -= Update;
        }

        private void Update()
        {
            var componentView = TargetAsComponentView();
            componentView.TryUpdateStoredValueFromEntity();
        }

        private ComponentView TargetAsComponentView() => (ComponentView) target;

        public override void OnInspectorGUI()
        {
            var componentView = TargetAsComponentView();

            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
                componentView.TryUpdateEntityFromStoredValue();

            DrawEntity(componentView);
            DrawComponentButtons(componentView);
        }

        private static void DrawEntity(ComponentView componentView)
        {
            if (!componentView.Entity.IsAliveCompatible()) return;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Entity", componentView.Entity.ToString());
            EditorGUI.EndDisabledGroup();

            var labelStyle = new GUIStyle(GUI.skin.label)
            {
                richText = true,
            };

            var componentStatus = componentView.EntityHasComponent()
                ? "<color=green>added</color>"
                : "<color=red>not added</color>";
            EditorGUILayout.LabelField($"Component: {componentStatus}.", labelStyle);
        }

        private void DrawComponentButtons(ComponentView componentView)
        {
            if (!componentView.Entity.IsAliveCompatible()) return;


            GUILayout.BeginHorizontal();
            RecordBackgroundColor();

            SetBackgroundColor(new Color(0.75f, 0.25f, 0.25f));

            EditorGUI.BeginDisabledGroup(!componentView.EntityHasComponent());
            if (GUILayout.Button("Delete component"))
                componentView.TryDeleteComponent();
            EditorGUI.EndDisabledGroup();

            SetBackgroundColor(new Color(0.25f, 0.75f, 0.25f));

            EditorGUI.BeginDisabledGroup(componentView.EntityHasComponent());
            if (GUILayout.Button("Add component"))
                componentView.TryAddComponent();
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