﻿#if UNITY_MODULES_PHYSICS
using DELTation.LeoEcsExtensions.Components;
using UnityEngine;

namespace DELTation.LeoEcsExtensions.Views.Components.Concrete
{
    public class CharacterControllerRefView : ComponentView<UnityRef<CharacterController>> { }
}
#endif