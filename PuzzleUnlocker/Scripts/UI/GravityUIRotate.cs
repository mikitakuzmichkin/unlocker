using UnityEngine;

namespace PuzzleUnlocker.UI
{
    public class GravityUIRotate : MonoBehaviour
    {
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            if (_transform.hasChanged)
            {
                _transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}