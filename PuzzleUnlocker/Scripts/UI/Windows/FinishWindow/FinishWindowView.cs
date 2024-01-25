using System;
using Dainty.UI.WindowBase;
using PuzzleUnlocker.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleUnlocker.UI.Windows
{
    public class FinishWindowView : AWindowView
    {
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private Button _skipButton;

        [Header("Top panel")] [SerializeField] private TextMeshProUGUI _text;

        [Space] [SerializeField] private Image _topPanelFront;
        [SerializeField] private Color _frontColorWin;
        [SerializeField] private Color _frontColorLose;

        [Space] [SerializeField] private Image _topPanelBack;
        [SerializeField] private Color _backColorWin;
        [SerializeField] private Color _backColorLose;

        public Action RestartClick;
        public Action SkipLevelClick;

        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            _restartButton.onClick.AddListener((() => RestartClick?.Invoke()));
            _skipButton.onClick.AddListener(() => SkipLevelClick?.Invoke());
        }

        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            _restartButton.onClick.RemoveAllListeners();
            _skipButton.onClick.RemoveAllListeners();
        }

        public void PlayConfetti()
        {
            _particle.Play();
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void SetButtonText(string text)
        {
            _buttonText.text = text;
        }

        public void SetSkipButtonActive(bool active)
        {
            _skipButton.gameObject.SetActive(active);
        }

        public void SetGameResult(EGameResult result)
        {
            switch (result)
            {
                case EGameResult.Win:
                    _topPanelFront.color = _frontColorWin;
                    _topPanelBack.color = _backColorWin;
                    break;
                case EGameResult.Lose:
                    _topPanelFront.color = _frontColorLose;
                    _topPanelBack.color = _backColorLose;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }
    }
}