#if UNITY_MODULES_PHYSICS_2D
using DELTation.LeoEcsExtensions.Components;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Views.Components.Concrete
{
    public class Rigidbody2dRefView : ComponentView<UnityRef<Rigidbody2D>> { }
}
#endif