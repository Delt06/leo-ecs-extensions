using UnityEngine;

namespace Runner.Collection
{
    public class CoinSpinAnimation : MonoBehaviour
    {
        [SerializeField] private Vector3 _angularVelocity;

        private void Update()
        {
            transform.Rotate(_angularVelocity * Time.deltaTime);
        }
    }
}