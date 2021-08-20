using DELTation.LeoEcsExtensions.Views;
using JetBrains.Annotations;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Pooling
{
	public interface IEntityViewPool
	{
		EntityView Create(Vector3 position, Quaternion rotation);
		void Dispose([NotNull] EntityView instance);
	}

	public interface IEntityViewPool<T> where T : IEntityView
	{
		T Create(Vector3 position, Quaternion rotation);
		void Dispose([NotNull] T instance);
	}
}