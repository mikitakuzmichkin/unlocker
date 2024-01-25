using System.Linq;
using PuzzleUnlocker.Scripts.Extensions;
using UnityEngine;

namespace PuzzleUnlocker.Editor
{
    public class MeshScalerByRectArea : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private EResolutionType _resolutionType;
        private void Start()
        {
            ChangeSize();
        }

        private void ChangeSize()
        {
            float meshSize;
            EResolutionType type = _resolutionType;
            var size = _rect.rect.size;
            var lossyScale = _rect.transform.lossyScale;
            size.x *= lossyScale.x;
            size.y *= lossyScale.y;
            if (_resolutionType == EResolutionType.Min)
            {
                type = size.x < size.y ? EResolutionType.Width : EResolutionType.Height;
            }
            switch (type)
            {
                case EResolutionType.Width:
                    meshSize = GetComponentsInChildren<MeshFilter>().Max(m => m.mesh.bounds.size.x * m.transform.lossyScale.x);
                    transform.localScale *= (size.x / meshSize);
                    break;
                case EResolutionType.Height:
                    meshSize = GetComponentsInChildren<MeshFilter>().Max(m => m.mesh.bounds.size.y * m.transform.lossyScale.y);
                    transform.localScale *= (size.y / meshSize);
                    break;
            }
            

        }

        
        public enum EResolutionType
        {
            Width,
            Height,
            Min
        }
    }
}