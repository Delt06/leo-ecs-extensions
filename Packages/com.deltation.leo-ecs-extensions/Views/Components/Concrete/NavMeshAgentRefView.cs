#if UNITY_MODULES_AI
using DELTation.LeoEcsExtensions.Components;
using UnityEngine.AI;

namespace DELTation.LeoEcsExtensions.Views.Components.Concrete
{
    public class NavMeshAgentRefView : ComponentView<UnityRef<NavMeshAgent>> { }
}
#endif