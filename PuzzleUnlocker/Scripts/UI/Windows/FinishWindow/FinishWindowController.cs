using System;
using Dainty.Ads;
using Dainty.DI;
using Dainty.UI.WindowBase;
using PuzzleUnlocker.Gameplay;
using PuzzleUnlocker.Services.Ads;

namespace PuzzleUnlocker.UI.Windows
{
    public class FinishWindowController : AWindowController<FinishWindowView>, IConfigurableWindow<FinishWindowSettings>
    {
        private const string SKIP_LEVEL_PLACEMENT = "skip_level";

        private readonly IAdsController _adsController;

        private FinishWindowSettings _settings;

        public FinishWindowController()
        {
            _adsController = ProjectContext.GetInstance<IAdsController>();
        }

        public override string WindowId { get; }

        public void Initialize(FinishWindowSettings data)
        {
            _settings = data;

            view.SetGameResult(data.Result);
            view.SetText(data.Result == EGameResult.Win ? "AWESOME" : "LOSE");
            view.SetButtonText(data.Result == EGameResult.Win ? "Next" : "Restart");

            if (data.Result == EGameResult.Win)
            {
                view.SetSkipButtonActive(false);
                view.PlayConfetti();
            }
            else
            {
                if (data.IsTutorial)
                {
                    view.SetSkipButtonActive(false);
                }
                else
                {
                    view.SetSkipButtonActive(_adsController.CanShow(AdType.Rewarded));
                }
            }
        }

        protected override void OnSubscribe()
        {
            base.OnSubscribe();
            view.RestartClick += RestartClick;
            view.SkipLevelClick += OnSkipLevelClick;
        }

        protected override void OnUnSubscribe()
        {
            base.OnUnSubscribe();
            view.RestartClick -= RestartClick;
            view.SkipLevelClick -= OnSkipLevelClick;
        }

        private void RestartClick()
        {
            _settings?.RestartAction?.Invoke();
            uiManager.Back();
        }

        private void OnSkipLevelClick()
        {
            if (_settings != null)
            {
                _adsController.ShowRewardedOnLevel(_settings.LevelId, SKIP_LEVEL_PLACEMENT, OnRewardedFinished, null);
            }
            else
            {
                _adsController.ShowRewarded(SKIP_LEVEL_PLACEMENT, OnRewardedFinished, null);
            }
        }

        private void OnRewardedFinished(bool result)
        {
            if (result)
            {
                _settings?.SkipLevelAction?.Invoke();
            }
            else
            {
                view.SetSkipButtonActive(false);
            }
        }
    }
}