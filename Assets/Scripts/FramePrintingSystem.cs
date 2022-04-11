using Leopotam.EcsLite;
using UnityEngine;

public class FramePrintingSystem : IEcsRunSystem
{
    public void Run(EcsSystems systems)
    {
        Debug.Log($"Frame: {Time.frameCount}");
    }
}