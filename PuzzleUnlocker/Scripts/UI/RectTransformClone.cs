using System;
using UnityEngine;

namespace PuzzleUnlocker.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformClone : MonoBehaviour
    {
        [SerializeField] private RectTransform _origin;
        [SerializeField] private bool _checkX;
        [SerializeField] private bool _checkY;
        [SerializeField] private bool _checkWidth;
        [SerializeField] private bool _checkHeight;

        private RectTransform _clone;

        private void Awake()
        {
            _clone = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (_checkX || _checkY)
            {
                bool xChanged = Math.Abs(transform.position.x - _origin.position.x) > 0.0001f;
                bool yChanged = Math.Abs(transform.position.y - _origin.position.y) > 0.0001f;
                if (xChanged || yChanged)
                {
                    var pos = transform.position;
                    if (_checkX && xChanged)
                    {
                        pos.x = _origin.transform.position.x;
                    }

                    if (_checkY && yChanged)
                    {
                        pos.y = _origin.transform.position.y;
                    }

                    transform.position = pos;
                }
            }

            if (_checkWidth || _checkHeight)
            {
                bool widthChanged = Math.Abs(_clone.sizeDelta.x - _origin.sizeDelta.x) > 0.0001f;
                bool heightChanged = Math.Abs(_clone.sizeDelta.y - _origin.sizeDelta.y) > 0.0001f;
                if (_checkWidth == false && _checkHeight == false)
                {
                    return;
                }

                var scale = _clone.sizeDelta;
                if (_checkWidth && widthChanged)
                {
                    scale.x = _origin.sizeDelta.x;
                }

                if (_checkHeight && heightChanged)
                {
                    scale.y = _origin.sizeDelta.y;
                }

                _clone.sizeDelta = scale;
            }
        }
    }
}