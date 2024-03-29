﻿using DELTation.LeoEcsExtensions.Utilities;
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

#if ODIN_INSPECTOR
        protected override void OnEnable()
        {
            base.OnEnable();
            OnEnableInternal();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnDisableInternal();
        }

#else
        protected virtual void OnEnable()
        {
            OnEnableInternal();
        }

        protected virtual void OnDisable()
        {
            OnDisableInternal();
        }

#endif

        private void OnEnableInternal()
        {
            Update();
            EditorApplication.update += Update;
        }

        private void OnDisableInternal()
        {
            EditorApplication.update -= Update;
        }

        private void Update()
        {
            if (TryGetTargetAsComponentView(out var componentView))
                componentView.TryUpdateDisplayedValueFromEntity();
        }

        private bool TryGetTargetAsComponentView(out ComponentView componentView)
        {
            componentView = target as ComponentView;
            return componentView != null;
        }

        public override void OnInspectorGUI()
        {
            if (!TryGetTargetAsComponentView(out var componentView)) return;

            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
                componentView.TryUpdateEntityFromDisplayedValue();

            DrawEntity(componentView);
            DrawComponentButtons(componentView);
        }

        private static void DrawEntity(ComponentView componentView)
        {
            if (!componentView.Entity.IsAlive()) return;

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
            if (!componentView.Entity.IsAlive()) return;


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