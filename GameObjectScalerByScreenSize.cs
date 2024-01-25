using UnityEngine;

namespace PuzzleUnlocker.Editor
{
    public class GameObjectScalerByScreenSize : MonoBehaviour
    {
        [SerializeField] private Vector2 _referenceResolution;
        [SerializeField] private EResolutionType _resolutionType;

        private void Start()
        {
            var aspect = Camera.main.aspect;
            if ((aspect > 1 && _resolutionType == EResolutionType.Min) || _resolutionType == EResolutionType.Height)
            {
                aspect = 1 / aspect;
            }

            var mul = aspect / GetAspectRatio(_referenceResolution.x, _referenceResolution.y);
            transform.localScale *= mul;
        }

        private float GetAspectRatio(float x, float y)
        {
            if ((_resolutionType == EResolutionType.Min && x > y) || _resolutionType == EResolutionType.Height)
            {
                return y / x;
            }

            return x / y;
        }

        public enum EResolutionType
        {
            Width,
            Height,
            Min
        }
    }
}