using UnityEngine;

namespace Runner._Shared.Utils
{
    public static class Bezier
    {
        public static Vector3 Quadratic(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            t = Mathf.Clamp01(t);
            var oneMinusT = 1 - t;
            return oneMinusT * oneMinusT * p0 + 2 * oneMinusT * t * p1 + t * t * p2;
        }

        public static Vector3 QuadraticDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            t = Mathf.Clamp01(t);
            return 2 * (1 - t) * (p1 - p0) + 2 * t * (p2 - p1);
        }
    }
}