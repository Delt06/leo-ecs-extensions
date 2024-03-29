﻿using System;
using DELTation.DIFramework;
using DELTation.LeoEcsExtensions.Composition.Di;
using DELTation.LeoEcsExtensions.Utilities;
using DELTation.LeoEcsExtensions.Views;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using UnityEditor;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Editor
{
    [CustomEditor(typeof(EntityView), true)]
    public class EntityViewEditor : UnityEditor.Editor
    {
        private bool _foldout;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var entityView = (EntityView) target;
            DrawEntity(entityView);
        }

        private void DrawEntity(IEntityView entityView)
        {
            if (!entityView.TryGetEntity(out var entity)) return;

            DrawEntityLabel(entity);

            _foldout = EditorGUILayout.Foldout(_foldout, "Components");
            if (!_foldout) return;

            DrawComponentTable(entity);
        }

        private static void DrawEntityLabel(EcsPackedEntityWithWorld entity)
        {
            EditorGUI.BeginDisabledGroup(true);

            const string label = "Entity";
            if (TryGetDebugEntityView(entity, out var entityDebugView))
                EditorGUILayout.ObjectField(label, entityDebugView, entityDebugView.GetType(), true);
            else
                EditorGUILayout.TextField(label, entity.ToString());

            EditorGUI.EndDisabledGroup();
        }

        private static bool TryGetDebugEntityView(EcsPackedEntityWithWorld entity,
            out EcsEntityDebugView entityDebugView)
        {
            entityDebugView = default;
            if (!entity.Unpack(out var world, out var idx)) return false;

            if (!Di.TryResolveGlobally(out EcsEntryPoint ecsEntryPoint))
                return false;
            if (ecsEntryPoint.World != world)
                return false;
            if (ecsEntryPoint.DebugSystem == null)
                return false;

            entityDebugView = ecsEntryPoint.DebugSystem.GetEntityView(idx);
            return entityDebugView != null;
        }

        private static void DrawComponentTable(EcsPackedEntityWithWorld entity)
        {
            var componentTypes = entity.GetComponentTypesCompatible();

            EditorGUI.indentLevel++;
            EditorGUI.BeginDisabledGroup(true);


            const int columns = 3;

            bool IsLastElementInRow(int i) => (i + 1) % columns == 0;
            bool IsLastComponent(int i) => i == componentTypes.Length - 1;
            bool ShouldEndRow(int i) => IsLastElementInRow(i) || IsLastComponent(i);

            var oldBgColor = GUI.backgroundColor;

            for (var index = 0; index < componentTypes.Length; index++)
            {
                if (index % columns == 0)
                    EditorGUILayout.BeginHorizontal();

                var componentType = componentTypes[index];
                if (componentType != null)
                {
                    GUI.backgroundColor = componentType.GetColor();
                    DrawComponentCell(componentType);
                }

                if (ShouldEndRow(index)) EditorGUILayout.EndHorizontal();
            }

            GUI.backgroundColor = oldBgColor;

            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel--;
        }

        private static void DrawComponentCell(Type componentType)
        {
            var content = componentType.GetFriendlyName();
            GUILayout.Button(content);
        }
    }
}