using Leopotam.Ecs;
using UnityEngine;

public class StartSystem : IEcsInitSystem
{
    public void Init()
    {
        Debug.Log("Game started");
    }
}