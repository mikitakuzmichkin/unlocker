using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PuzzleUnlocker.Scripts.UI
{
    public class ToggleOutImage : MonoBehaviour, IPointerClickHandler
    {
        public class ToggleEvent : UnityEvent<bool>
        {}
        
        [SerializeField] private Graphic _targetGraphic;
        [SerializeField] private Image _changingImage;
        [SerializeField] private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;
        [SerializeField][InspectorName("IsOn")] private bool _isOnStart;

        public ToggleEvent OnValueChanged  = new ToggleEvent();
        
        private bool _isOn;

        protected  void Awake()
        {
            ChangeValue(_isOnStart);
        }

        public void ChangeValue(bool isOn)
        {
            _isOn = isOn;
            if (_changingImage != null)
            {
                _changingImage.sprite = isOn ? _onSprite : _offSprite;
            }
            OnValueChanged?.Invoke(_isOn);
        }
        
        private void InternalToggle()
        {
            ChangeValue(!_isOn);
        }

        /// <summary>
        /// React to clicks.
        /// </summary>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }
            
            if (_targetGraphic.raycastTarget == false)
            {
                return;
            }

            InternalToggle();
        }

        
#if UNITY_EDITOR
        private void OnValidate()
        {
            ChangeValue(_isOnStart);
        }
#endif
    }
}