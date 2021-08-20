﻿namespace DELTation.LeoEcsExtensions.Pooling
{
	public struct PoolBackRef
	{
		public EntityViewPool Pool;

		public static implicit operator EntityViewPool(PoolBackRef backRef) => backRef.Pool;
	}
}