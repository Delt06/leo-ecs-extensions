#if LEOECS_EXTENSIONS_LITE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DELTation.LeoEcsExtensions.Composition;
using DELTation.LeoEcsExtensions.Systems;
using DELTation.LeoEcsExtensions.Utilities;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Editor
{
    [CustomEditor(typeof(EcsEntryPoint), true)]
    public class EcsEntryPointEditor : UnityEditor.Editor
    {
        private bool _lateSystemsExpanded;
        private bool _physicsSystemsExpanded;
        private bool _systemsExpanded;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var entryPoint = (EcsEntryPoint) target;

            TryDrawSystems(entryPoint.Systems, "Systems", ref _systemsExpanded);
            TryDrawSystems(entryPoint.PhysicsSystems, "Physics Systems", ref _physicsSystemsExpanded);
            TryDrawSystems(entryPoint.LateSystems, "Late Systems", ref _lateSystemsExpanded);
        }

        private static void TryDrawSystems(EcsSystems systemsCollection, string label, ref bool expanded)
        {
            if (systemsCollection == null) return;

            IEcsSystem[] systems = null;
            systemsCollection.GetAllSystems(ref systems);
            if (systems == null) return;
            if (systems.Length == 0) return;

            expanded = EditorGUILayout.Foldout(expanded, label);
            if (!expanded) return;

            EditorGUI.indentLevel++;


            foreach (var system in systems)
            {
                if (system == null) continue;


                var systemName = system.GetType().GetFriendlyName();

                var oldBgColor = GUI.backgroundColor;
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField(systemName, GUILayout.Width(250));

                GUILayout.Space(16);

                foreach (var (type, accessType) in AccessAnalysis.Analyze(system.GetType()))
                {
                    var color = GetAccessTypeColor(accessType);
                    GUI.backgroundColor = color;
                    var friendlyName = type.GetFriendlyName();
                    GUILayout.Button($"{friendlyName} [{accessType.ToShortString()}]", GUILayout.Width(250));
                }

                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
                GUI.backgroundColor = oldBgColor;
            }


            EditorGUI.indentLevel--;
        }

        private static Color GetAccessTypeColor(ComponentAccessType componentAccessType) =>
            componentAccessType switch
            {
                ComponentAccessType.Unstructured => Color.blue,
                ComponentAccessType.ReadOnly => Color.green,
                ComponentAccessType.ReadWrite => new Color(1f, 0.5f, 0f),
                _ => throw new ArgumentOutOfRangeException(),
            };
    }
}

internal static class AccessAnalysis
{
    public static string ToShortString(this ComponentAccessType componentAccessType) =>
        componentAccessType switch
        {
            ComponentAccessType.Unstructured => "U",
            ComponentAccessType.ReadOnly => "R",
            ComponentAccessType.ReadWrite => "RW",
            _ => throw new ArgumentOutOfRangeException(nameof(componentAccessType), componentAccessType, null),
        };

    public static (Type type, ComponentAccessType accessType)[] Analyze([NotNull] Type systemType)
    {
        if (systemType == null) throw new ArgumentNullException(nameof(systemType));

        var results = new List<(Type type, ComponentAccessType accessType)>();

        var systemComponentAccessAttributes = systemType.GetCustomAttributes<SystemComponentAccessAttribute>();

        foreach (var attribute in systemComponentAccessAttributes)
        {
            results.Add((attribute.Type, attribute.AccessType));
        }

        var fields = systemType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            var fieldType = field.FieldType;
            if (!fieldType.IsConstructedGenericType) continue;

            var genericTypeDefinition = fieldType.GetGenericTypeDefinition();
            CheckGenericTypeAndAdd(genericTypeDefinition, fieldType, results);
        }

        var baseType = systemType.BaseType;
        if (baseType != null && baseType != typeof(object))
            return results.Concat(Analyze(baseType)).ToArray();

        return results.ToArray();
    }

    private static void CheckGenericTypeAndAdd(Type genericTypeDefinition, Type fieldType,
        List<(Type type, ComponentAccessType accessType)> results)
    {
        if (genericTypeDefinition == typeof(EcsPool<>))
        {
            var componentType = fieldType.GenericTypeArguments[0];
            results.Add((componentType, ComponentAccessType.Unstructured));
        }
        else if (genericTypeDefinition == typeof(EcsReadOnlyPool<>))
        {
            var componentType = fieldType.GenericTypeArguments[0];
            results.Add((componentType, ComponentAccessType.ReadOnly));
        }
        else if (genericTypeDefinition == typeof(EcsReadWritePool<>))
        {
            var componentType = fieldType.GenericTypeArguments[0];
            results.Add((componentType, ComponentAccessType.ReadWrite));
        }
    }
}

#endif