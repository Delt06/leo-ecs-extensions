namespace DELTation.LeoEcsExtensions.Components
{
	public struct ViewBackRef<TView>
	{
		public TView View;

		public static implicit operator TView(ViewBackRef<TView> backRef) => backRef.View;
	}
}