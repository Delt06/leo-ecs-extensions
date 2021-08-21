using Leopotam.Ecs;
using UnityEngine;

public class FramePrintingSystem : IEcsRunSystem
{
    public void Run()
    {
        Debug.Log($"Frame: {Time.frameCount}");
    }
}