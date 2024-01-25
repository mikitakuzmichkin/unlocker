using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PuzzleUnlocker.UI
{
    public class LongPressButton : Selectable, IPointerClickHandler, ISubmitHandler
    {
        [SerializeField] private bool m_LongPressEnabled = true;
        [SerializeField] private float m_LongPressTime = 0.2f;

        [Space] [SerializeField] private UnityEvent m_OnClick = new UnityEvent();
        [SerializeField] private UnityEvent m_OnLongPress = new UnityEvent();

        private Coroutine _longPressTimerCoroutine;
        private bool _skipClick;

        public bool longPressEnabled
        {
            get => m_LongPressEnabled;
            set
            {
                m_LongPressEnabled = value;
                StopLongPressTimer();
            }
        }

        public float longPressTime
        {
            get => m_LongPressTime;
            set => m_LongPressTime = value;
        }

        public UnityEvent onClick
        {
            get => m_OnClick;
            set => m_OnClick = value;
        }

        public UnityEvent onLongPress
        {
            get => m_OnLongPress;
            set => m_OnLongPress = value;
        }

        protected LongPressButton()
        {
        }

        #region IPointer*Handler

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (!m_LongPressEnabled || eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            _skipClick = false;
            StartLongPressTimer();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            StopLongPressTimer();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (_longPressTimerCoroutine != null)
            {
                StopCoroutine(_longPressTimerCoroutine);
                _longPressTimerCoroutine = null;
            }
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            if (!_skipClick)
            {
                Press();
            }
        }

        #endregion

        public virtual void OnSubmit(BaseEventData eventData)
        {
            Press();

            // if we get set disabled during the press
            // don't run the coroutine.
            if (!IsActive() || !IsInteractable())
            {
                return;
            }

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _longPressTimerCoroutine = null;
            _skipClick = false;
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable())
            {
                return;
            }

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            m_OnClick.Invoke();
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }

        private void StartLongPressTimer()
        {
            if (_longPressTimerCoroutine != null)
            {
                StopCoroutine(_longPressTimerCoroutine);
            }

            _longPressTimerCoroutine = StartCoroutine(LongPressTimer());
        }

        private void StopLongPressTimer()
        {
            if (_longPressTimerCoroutine != null)
            {
                StopCoroutine(_longPressTimerCoroutine);
                _longPressTimerCoroutine = null;
            }
        }

        private IEnumerator LongPressTimer()
        {
            var longPressTime = m_LongPressTime;
            var elapsedTime = 0f;

            while (elapsedTime < longPressTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            _skipClick = true;
            _longPressTimerCoroutine = null;
            m_OnLongPress.Invoke();
        }
    }
}