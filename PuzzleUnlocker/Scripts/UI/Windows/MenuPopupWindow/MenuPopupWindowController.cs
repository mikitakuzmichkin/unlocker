using Dainty.DI;
using Dainty.UI.WindowBase;
using PuzzleUnlocker.Data;
using PuzzleUnlocker.Editor;
using PuzzleUnlocker.Gameplay;
using UnityEngine;

namespace PuzzleUnlocker.UI.Windows
{
    public class MenuPopupWindowController : AWindowController<MenuPopupWindowView>
    {
        public override string WindowId => WindowsId.MENU_POPUP_WINDOW;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            var vibrationSwitcher = ProjectContext.GetInstance<IPuDataProvider>().Vibration;
            view.SetVibration(vibrationSwitcher);
        }

        protected override void OnSubscribe()
        {
            view.ToggleChanged += SwitchVibration;
            view.TermsButton += ToTerms;
            view.PrivacyPolicyButton += ToPrivacyPolicy;
            view.CloseButton += ViewOnCloseButton;
        }

        protected override void OnUnSubscribe()
        {
            view.ToggleChanged -= SwitchVibration;
            view.TermsButton -= ToTerms;
            view.PrivacyPolicyButton -= ToPrivacyPolicy;
            view.CloseButton -= ViewOnCloseButton;
        }

        private void ViewOnCloseButton()
        {
            uiManager.Back();
        }

        private void SwitchVibration(bool value)
        {
            ProjectContext.GetInstance<IPuDataProvider>().Vibration = value;
        }

        public void ToTerms()
        {
            Application.OpenURL(Constants.URL_EULA);
        }

        public void ToPrivacyPolicy()
        {
            Application.OpenURL(Constants.URL_PRIVACY_POLICY);
        }
    }
}