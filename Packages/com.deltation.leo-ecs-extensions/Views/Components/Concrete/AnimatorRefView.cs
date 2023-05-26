#if UNITY_MODULES_ANIMATION
using DELTation.LeoEcsExtensions.Components;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Views.Components.Concrete
{
    public class AnimatorRefView : ComponentView<UnityRef<Animator>> { }
}
#endif