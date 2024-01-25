using System;
using Dainty.UI.WindowBase;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleUnlocker.UI.Windows
{
    public class DefaultPopupWindowAnimation : AWindowAnimation
    {
#pragma warning disable 649
        [SerializeField] private Canvas _canvas;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Graphic _backingBackground;
        [SerializeField] private Color _backingBackgroundColor = new Color(0, 0, 0, 0.4f);
        [SerializeField] private RectTransform _frame;
#pragma warning disable 649

        private Tween _tween;

        public override void ShowImmediate()
        {
            _tween?.Kill();
            _canvasGroup.alpha = 1;
            _backingBackground.color = _backingBackgroundColor;
            _frame.localScale = Vector3.one;
            _canvas.enabled = true;
        }

        public override void PlayShowAnimation(bool push, Action animationFinished = null)
        {
            _tween?.Kill();

            _canvasGroup.alpha = 1;
            _backingBackground.color = Color.clear;
            _frame.localScale = Vector3.one * 0.9f;

            _tween = DOTween.Sequence()
                .Join(_backingBackground
                    .DOColor(_backingBackgroundColor, WindowAnimationConstants.POPUPS_TRANSITION_TIME)
                    .SetEase(Ease.OutQuart))
                .Join(_frame.DOScale(Vector3.one, WindowAnimationConstants.POPUPS_TRANSITION_TIME)
                    .SetEase(Ease.OutQuart))
                .OnComplete(() => animationFinished?.Invoke());
        }

        public override void CloseImmediate()
        {
            _tween?.Kill();
            _canvas.enabled = false;
        }

        public override void PlayCloseAnimation(bool pop, Action animationFinished = null)
        {
            _tween?.Kill();

            _tween = DOTween.Sequence()
                .Join(_canvasGroup.DOFade(0, WindowAnimationConstants.POPUPS_TRANSITION_TIME)
                    .SetEase(Ease.OutQuart))
                .Join(_backingBackground.DOColor(Color.clear, WindowAnimationConstants.POPUPS_TRANSITION_TIME)
                    .SetEase(Ease.OutQuart))
                .OnComplete(() => animationFinished?.Invoke());
        }
    }
}