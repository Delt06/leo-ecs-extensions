#if LEOECS_EXTENSIONS_LITE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DELTation.LeoEcsExtensions.Composition.Di;
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
        private static bool _lateSystemsExpanded;
        private static bool _physicsSystemsExpanded;
        private static string _searchQuery;
        private static bool _systemsExpanded;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var entryPoint = (EcsEntryPoint) target;

            TryDrawSearchBar(entryPoint);
            TryDrawSystems(entryPoint.Systems, "Systems", ref _systemsExpanded, _searchQuery);
            TryDrawSystems(entryPoint.PhysicsSystems, "Physics Systems", ref _physicsSystemsExpanded, _searchQuery);
            TryDrawSystems(entryPoint.LateSystems, "Late Systems", ref _lateSystemsExpanded, _searchQuery);
        }

        private static void TryDrawSearchBar(EcsEntryPoint entryPoint)
        {
            if (entryPoint.Systems == null) return;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Filter (case-insensitive)", GUILayout.Width(175));
            _searchQuery = EditorGUILayout.TextField(_searchQuery, GUILayout.Width(250));
            EditorGUILayout.EndHorizontal();
        }

        private static void TryDrawSystems(EcsSystems systemsCollection, string label, ref bool expanded,
            string searchQuery)
        {
            if (systemsCollection == null) return;

            var systemsAnalysis = AnalyzeSystemsCollection(systemsCollection)
                .ToArray();
            if (systemsAnalysis.Length == 0) return;

            expanded = EditorGUILayout.Foldout(expanded, label);
            if (!expanded) return;

            EditorGUI.indentLevel++;


            foreach (var (system, systemAnalysis) in systemsAnalysis)
            {
                var isFiltering = !string.IsNullOrWhiteSpace(searchQuery);
                var filteredSystemAnalysis =
                    !isFiltering
                        ? systemAnalysis
                        : systemAnalysis.Where((t, _) =>
                            ShouldAppearInSearchResults(t, searchQuery)
                        ).ToArray();
                if (isFiltering && filteredSystemAnalysis.Length == 0) continue;

                var systemName = system.GetType().GetFriendlyName();

                var oldBgColor = GUI.backgroundColor;
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField(systemName, GUILayout.Width(250));

                GUILayout.Space(16);


                foreach (var t in filteredSystemAnalysis)
                {
                    var (color, text) = AnalysisResultToButtonAttributes(t);
                    GUI.backgroundColor = color;
                    GUILayout.Button(text, GUILayout.Width(250));
                }

                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
                GUI.backgroundColor = oldBgColor;
            }


            EditorGUI.indentLevel--;
        }

        private static (Color color, string text) AnalysisResultToButtonAttributes(
            (Type type, ComponentAccessType accessType)? result)
        {
            Color color;
            string text;
            if (result != null)
            {
                var (type, accessType) = result.Value;
                color = GetAccessTypeColor(accessType);
                text = ComponentWithAccessToString(type, accessType);
            }
            else
            {
                text = "Packed Filter [*]";
                color = Color.red;
            }

            return (color, text);
        }

        private static string ComponentWithAccessToString(Type componentType, ComponentAccessType accessType) =>
            $"{componentType.GetFriendlyName()} [{accessType.ToShortString()}]";

        private static bool ShouldAppearInSearchResults((Type componentType, ComponentAccessType accessType)? result,
            string searchQuery)
        {
            var (_, text) = AnalysisResultToButtonAttributes(result);
            return text.ToLower()
                .Contains(searchQuery.ToLower());
        }

        private static IEnumerable<(IEcsSystem system, (Type type, ComponentAccessType accessType)?[] analysis)>
            AnalyzeSystemsCollection(EcsSystems systemsCollection)
        {
            IEcsSystem[] systems = null;
            systemsCollection.GetAllSystems(ref systems);
            if (systems == null) yield break;
            if (systems.Length == 0) yield break;

            foreach (var system in systems)
            {
                if (system == null) continue;

                var systemAnalysis = AccessAnalysis.Analyze(system.GetType());
                yield return (system, systemAnalysis);
            }
        }

        private static Color GetAccessTypeColor(ComponentAccessType componentAccessType) =>
            componentAccessType switch
            {
                ComponentAccessType.Unstructured => Color.blue,
                ComponentAccessType.ReadOnly => Color.green,
                ComponentAccessType.ReadWrite => new Color(1f, 0.5f, 0f),
                ComponentAccessType.Observable => Color.magenta,
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
            ComponentAccessType.Observable => "O",
            _ => throw new ArgumentOutOfRangeException(nameof(componentAccessType), componentAccessType, null),
        };

    public static (Type type, ComponentAccessType accessType)?[] Analyze([NotNull] Type systemType)
    {
        if (systemType == null) throw new ArgumentNullException(nameof(systemType));

        var results = new List<(Type type, ComponentAccessType accessType)?>();

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
        List<(Type type, ComponentAccessType accessType)?> results)
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
        else if (genericTypeDefinition == typeof(EcsObservablePool<>))
        {
            var componentType = fieldType.GenericTypeArguments[0];
            results.Add((componentType, ComponentAccessType.Observable));
        }
    }
}

#endif