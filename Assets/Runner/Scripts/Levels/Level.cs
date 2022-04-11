using Runner._Shared.Utils;
using UnityEngine;

namespace Runner.Levels
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private Spline _spline;

        public Spline Spline => _spline;
    }
}