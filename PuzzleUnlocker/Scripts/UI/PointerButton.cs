using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PuzzleUnlocker.Scripts.UI
{
    public class PointerButton : MonoBehaviour, IPointerDownHandler
    {
        private Action<PointerEventData> _down;
        public void OnPointerDown(PointerEventData eventData)
        {
            _down?.Invoke(eventData);
        }

        public void AddListener(Action<PointerEventData> action)
        {
            _down += action;
        }
        
        public void RemoveAllListeners()
        {
            _down = null;
        }
    }
}