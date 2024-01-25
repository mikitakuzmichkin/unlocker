using Dainty.NativeUtils;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class CanvasScalerController : MonoBehaviour
    {
        public static readonly Vector2Int PhoneViewport = new Vector2Int(375, 667);
        public static readonly Vector2Int TabletViewport = new Vector2Int(768, 1024);

        [SerializeField] private CanvasScaler _canvasScaler;

        private void Awake()
        {
            if (_canvasScaler == null)
            {
                _canvasScaler = GetComponent<CanvasScaler>();
            }

            _canvasScaler.referenceResolution = NativeUtilsProvider.ScreenUtils.ViewportSize;
        }
    }
}