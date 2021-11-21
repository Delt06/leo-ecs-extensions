using Leopotam.EcsLite;
using UnityEngine;

public class StartSystem : IEcsInitSystem
{
    public void Init(EcsSystems systems)
    {
        Debug.Log("Game started");
    }
}