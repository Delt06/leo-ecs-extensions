using System;
using System.Collections.Generic;
using DELTation.DIFramework;
using Leopotam.Ecs;
#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Composition.Di
{
	public class FiltersDependencyContainer : MonoBehaviour, IDependencyContainer
	{
		[SerializeField, Required]
#if UNITY_EDITOR && ODIN_INSPECTOR
#endif
		private EcsEntryPoint _ecsEntryPoint = default;

		public bool TryResolve(Type type, out object dependency)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			if (!CanBeResolvedSafe(type))
			{
				dependency = default;
				return false;
			}

			dependency = _ecsEntryPoint.World.GetFilter(type);
			return true;
		}

		public bool CanBeResolvedSafe(Type type)
		{
			if (type == null) throw new ArgumentNullException(nameof(type));
			return typeof(EcsFilter).IsAssignableFrom(type);
		}

		public void GetAllRegisteredObjects(ICollection<object> objects) { }
	}
}