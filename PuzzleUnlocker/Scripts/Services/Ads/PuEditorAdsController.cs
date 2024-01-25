#if UNITY_EDITOR
using Dainty.Ads;
using UnityEngine;
using Utils;

namespace PuzzleUnlocker.Services.Ads
{
    public class PuEditorAdsController : EditorAdsController
    {
        protected override void OnInitialize(Canvas rootCanvas)
        {
            rootCanvas.gameObject.AddComponent<CanvasScalerController>();
        }
    }
}
#endif