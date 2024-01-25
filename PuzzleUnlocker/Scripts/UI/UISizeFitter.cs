using System.Collections;
using UnityEngine;

namespace PuzzleUnlocker.UI
{
    public class UISizeFitter : MonoBehaviour
    {
        private IEnumerator Start()
        {
            if (transform.parent == null)
            {
                yield break;
            }

            var rect = GetComponent<RectTransform>();
            var parentRect = transform.parent.GetComponent<RectTransform>();

            if (parentRect.rect.width == 0)
            {
                yield return new WaitUntil(() => parentRect.rect.width != 0);
            }

            CheckX(rect, parentRect);
        }

        private void CheckX(RectTransform rect, RectTransform parentRect)
        {
            if (rect.rect.width > parentRect.rect.width)
            {
                var difference = parentRect.rect.width / rect.rect.width;
                rect.localScale *= difference;
            }
        }

        private void CheckY(RectTransform rect, RectTransform parentRect)
        {
            if (rect.rect.height > parentRect.rect.height)
            {
                var difference = parentRect.rect.height / rect.rect.height;
                rect.localScale *= difference;
            }
        }
    }
}