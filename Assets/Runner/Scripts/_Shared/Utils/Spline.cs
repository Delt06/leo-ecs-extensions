using System.Linq;
using UnityEngine;

namespace Runner._Shared.Utils
{
    public class Spline : MonoBehaviour
    {
        private Vector3[] _splinePoints;

        private float MaxT => CurvesCount;

        private int CurvesCount => (_splinePoints.Length - 1) / 2;

        private void Awake()
        {
            Initialize();
        }

        private void OnDrawGizmos()
        {
            const float radius = 0.25f;

            for (var i = 0; i < transform.childCount - 2; i += 2)
            {
                var startPoint = transform.GetChild(i).position;
                var middlePoint = transform.GetChild(i + 1).position;
                var endPoint = transform.GetChild(i + 2).position;

                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(startPoint, middlePoint);
                Gizmos.DrawLine(middlePoint, endPoint);
                Gizmos.DrawWireSphere(middlePoint, radius);

                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(startPoint, radius);
                Gizmos.DrawWireSphere(endPoint, radius);
                GizmosHelper.DrawBezier(startPoint, middlePoint, endPoint);
            }
        }

        private void Initialize()
        {
            _splinePoints = transform.OfType<Transform>()
                .Select(t => t.position)
                .ToArray();
        }

        public (Vector3 point, float newT) SamplePoint(float t, float deltaDistance)
        {
            const float tStep = 0.001f;
            const int maxSteps = 100;

            var cumulativeDistance = 0f;
            var previousPoint = SamplePoint(t);

            for (var i = 0; i < maxSteps; i++)
            {
                t += tStep;
                if (t >= MaxT)
                {
                    t = MaxT;
                    previousPoint = SamplePoint(t);
                    break;
                }

                var point = SamplePoint(t);
                var distance = Vector3.Distance(previousPoint, point);
                cumulativeDistance += distance;
                if (cumulativeDistance >= deltaDistance)
                    return (point, t);

                previousPoint = point;
            }

            return (previousPoint, t);
        }

        public Vector3 SampleDerivative(float t)
        {
            var (relativeT, startPoint, middlePoint, endPoint) = Sample(t);
            return Bezier.QuadraticDerivative(startPoint, middlePoint, endPoint, relativeT).normalized;
        }

        public Vector3 SamplePoint(float t)
        {
            var (relativeT, startPoint, middlePoint, endPoint) = Sample(t);
            return Bezier.Quadratic(startPoint, middlePoint, endPoint, relativeT);
        }

        private (float relativeT, Vector3 startPoint, Vector3 middlePoint, Vector3 endPoint) Sample(float t)
        {
            var curvesCount = CurvesCount;
            var curveIndex = Mathf.FloorToInt(t);
            curveIndex = Mathf.Clamp(curveIndex, 0, curvesCount - 1);

            var relativeT = t - curveIndex;
            var startPoint = _splinePoints[curveIndex * 2];
            var middlePoint = _splinePoints[curveIndex * 2 + 1];
            var endPoint = _splinePoints[curveIndex * 2 + 2];
            return (relativeT, startPoint, middlePoint, endPoint);
        }
    }
}