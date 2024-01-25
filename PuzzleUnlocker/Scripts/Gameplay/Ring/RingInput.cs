using System;
using PuzzleUnlocker.Scripts.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PuzzleUnlocker.Gameplay.Ring
{
    public class RingInput : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler
    {
        private static RingInput _instance;
        private float _prevAngle;

        public static RingInput Instance => _instance;

        public event Action Press;
        public event Action<float> Rotate;
        
        public bool EndDrag { get; set; }

        private void Awake()
        {
            _instance = this;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (EndDrag)
            {
                EndDrag = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            var pos = eventData.pressEventCamera.ScreenToWorldPoint(eventData.position) - transform.position;
            var nextAngle = GetAngleFromPos(pos);
            Rotate?.Invoke(Mathf.DeltaAngle(_prevAngle, nextAngle));
            _prevAngle = nextAngle;

            if (EndDrag)
            {
                EndDrag = false;
                eventData.pointerDrag = null;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var pos = eventData.pressEventCamera.ScreenToWorldPoint(eventData.position) - transform.position;
            _prevAngle = GetAngleFromPos(pos);
            Press?.Invoke();
        }

        private static float GetAngleFromPos(Vector2 pos)
        {
            pos.Normalize();
            var cos = pos.x;
            var sin = pos.y;

            var angle = Mathf.Atan(sin / cos) * Mathf.Rad2Deg;
            if (cos < 0)
            {
                angle += 180;
            }
            else if (sin < 0)
            {
                angle += 360;
            }

            return angle;
        }
    }
}