using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Views
{
    internal static class EntityViewUtils
    {
        public static void FindAllEntityInitializers([NotNull] Transform root,
            [NotNull] List<IEntityInitializer> initializers)
        {
            if (root == null) throw new ArgumentNullException(nameof(root));
            if (initializers == null) throw new ArgumentNullException(nameof(initializers));
            FindAllEntityInitializers(root, initializers, false);
        }

        private static void FindAllEntityInitializers([NotNull] Transform root,
            [NotNull] List<IEntityInitializer> initializers, bool checkForViews)
        {
            if (checkForViews && root.TryGetComponent(out IEntityView _)) return;

            root.GetComponents(Buffer);

            for (var index = 0; index < Buffer.Count; index++)
            {
                initializers.Add(Buffer[index]);
            }

            Buffer.Clear();

            for (int childIndex = 0, childCount = root.childCount; childIndex < childCount; childIndex++)
            {
                var child = root.GetChild(childIndex);
                FindAllEntityInitializers(child, initializers, true);
            }
        }

        private static readonly List<IEntityInitializer> Buffer = new List<IEntityInitializer>();
    }
}