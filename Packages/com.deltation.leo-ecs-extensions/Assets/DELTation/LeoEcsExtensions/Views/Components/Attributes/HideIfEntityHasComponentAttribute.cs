namespace DELTation.LeoEcsExtensions.Views.Components.Attributes
{
    internal class HideIfEntityHasComponentAttribute : ShowIfEntityHasComponentAttributeBase
    {
        public HideIfEntityHasComponentAttribute() => IsInverted = true;
    }
}