using System;
using DELTation.LeoEcsExtensions.Views;

namespace DELTation.LeoEcsExtensions.Components
{
    [Serializable]
    public struct ViewBackRef<TView>
    {
        public TView View;

        public static implicit operator TView(ViewBackRef<TView> backRef) => backRef.View;
    }

    [Serializable]
    public struct ViewBackRef
    {
        public IEntityView View;
    }
}