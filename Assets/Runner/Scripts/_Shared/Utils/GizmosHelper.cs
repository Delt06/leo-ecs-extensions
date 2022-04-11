using UnityEngine;

namespace Runner._Shared.Utils
{
    public static class GizmosHelper
    {
        public static void DrawBezier(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            const float tStep = 0.05f;

            for (var t = 0f; t <= 1; t += tStep)
            {
                var lineEnd1 = Bezier.Quadratic(p0, p1, p2, t);
                var lineEnd2 = Bezier.Quadratic(p0, p1, p2, t + tStep);
                Gizmos.DrawLine(lineEnd1, lineEnd2);
            }
        }
    }
}