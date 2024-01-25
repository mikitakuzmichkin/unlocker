using System;
using Dainty.UI.WindowBase;
using PuzzleUnlocker.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleUnlocker.UI.Windows
{
    public class MenuPopupWindowView : AWindowView
    {
        [SerializeField] private ToggleOutImage _vibrationToggle;
        [SerializeField] private Button _privacyPolicyButton;
        [SerializeField] private Button _termsButton;
        [SerializeField] private Button[] _closeButtons;

        public event Action CloseButton;
        public event Action PrivacyPolicyButton;
        public event Action TermsButton;
        public event Action<bool> ToggleChanged;

        public void SetVibration(bool value)
        {
            _vibrationToggle.ChangeValue(value);
        }

        protected override void OnSubscribe()
        {
            _vibrationToggle.OnValueChanged.AddListener((b) => ToggleChanged?.Invoke(b));
            _privacyPolicyButton.onClick.AddListener(() => PrivacyPolicyButton?.Invoke());
            _termsButton.onClick.AddListener(() => TermsButton?.Invoke());
            foreach (var closeButton in _closeButtons)
            {
                closeButton.onClick.AddListener(() => CloseButton?.Invoke());
            }
        }

        protected override void OnUnSubscribe()
        {
            _vibrationToggle.OnValueChanged.RemoveAllListeners();
            _privacyPolicyButton.onClick.RemoveAllListeners();
            _termsButton.onClick.RemoveAllListeners();
            foreach (var closeButton in _closeButtons)
            {
                closeButton.onClick.RemoveAllListeners();
            }
        }
    }
}