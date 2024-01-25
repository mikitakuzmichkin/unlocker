using System;
using Dainty.Ads;
using Dainty.DI;
using Dainty.NativeUtils;
using Dainty.UI.WindowBase;
using GameAnalyticsSDK;
using PuzzleUnlocker.Data;
using PuzzleUnlocker.Gameplay;
using PuzzleUnlocker.Gameplay.Builder;
using PuzzleUnlocker.Gameplay.Level;
using PuzzleUnlocker.Gameplay.Model;
using PuzzleUnlocker.Gameplay.Ring;
using PuzzleUnlocker.Tutorials;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PuzzleUnlocker.UI.Windows
{
    public class GameWindowController : AWindowController<GameWindowView>
    {
        public override string WindowId => WindowsId.GAMEPLAY_WINDOW;

        private readonly IAdsController _adsController;
        private readonly IPuDataProvider _dataProvider;
        private readonly ILevelDataProvider _levelProvider;

        private IGameplayModel _model;
        private IRingController _ringController;

        private int _curLevel;
        private bool _readyToPlay;

        private int _ballCollisionCount;

        private TutorialController _tutorialController;
        private bool _isTutorial;

        public GameWindowController()
        {
            _adsController = ProjectContext.GetInstance<IAdsController>();
            _dataProvider = ProjectContext.GetInstance<IPuDataProvider>();
            _levelProvider = ProjectContext.GetInstance<ILevelDataProvider>();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _tutorialController = new TutorialController();
            _tutorialController.Init(view.TutorialView);
            InitLevel();
        }

        private void InitLevel()
        {
            ReleaseLevel();

#if UNITY_EDITOR
            _curLevel = view.IsUseLevelFromEditor
                ? view.Level
                : _dataProvider.CurrentLevel;
#else
            _curLevel = _dataProvider.CurrentLevel;
#endif

            var tutorLevel = _tutorialController.TryGetTutorialLevels(_curLevel);
            _isTutorial = tutorLevel != null;

            ALevelData level;

            var totalLevelsCount = _levelProvider.GetLength();
            if (!_isTutorial && _curLevel >= totalLevelsCount)
            {
                var lastLevel = _curLevel;
                _curLevel = Random.Range(0, totalLevelsCount);
                level = _levelProvider.GetLevelData(_curLevel);

                view.SetLevelText("Max Reached");

                if (_dataProvider.AllLevelsPassedPopupShownAt < lastLevel)
                {
                    NativeUtilsProvider.ApplicationUtils.CreateNativeDialog()
                        .SetTitle("Congratulations, you have completed all levels!")
                        .SetMessage(
                            "It was sudden, but very soon we will add new ones...\nIn the meantime, you can play at a random level!")
                        .SetNeutralButton("Ok")
                        .Show(null);

                    _dataProvider.AllLevelsPassedPopupShownAt = lastLevel;
                }
            }
            else
            {
                if (_isTutorial)
                {
                    level = tutorLevel;
                    _adsController.HideBanner();
                }
                else
                {
                    level = _levelProvider.GetLevelData(_curLevel);
                }

                var curTextLevel = _curLevel + _tutorialController.GetLevelsBefore(_curLevel, _isTutorial);
                var text =
#if DEV && !MOTION_DESIGN
                    _isTutorial
                        ? $"Level {curTextLevel + 1} (id: -)"
                        : $"Level {curTextLevel + 1} (id: {(level.Id)})";
#else
                    $"Level {curTextLevel + 1}";
#endif
                view.SetLevelText(text);
            }

            GameplayBuilder builder = null;
            switch (level.GameType)
            {
                case EGameType.Number:
                {
                    var builderNumber = new GameplayNumberBuilder();
                    builderNumber.Build(view, level, out var ringController);
                    _ringController = ringController;
                    builder = builderNumber;
                    break;
                }
                case EGameType.Cube:
                {
                    var builderCube = new GameplayCubeBuilder();
                    builderCube.Build(view, level, out var ringController);
                    _ringController = ringController;
                    builder = builderCube;
                    break;
                }
                case EGameType.Color:
                {
                    var builderColor = new GameplayColorBuilder();
                    builderColor.Build(view, level, out var ringController);
                    _ringController = ringController;
                    builder = builderColor;
                    break;
                }
                case EGameType.Figure:
                {
                    var builderFigure = new GameplayFigureBuilder();
                    builderFigure.Build(view, level, out var ringController);
                    _ringController = ringController;
                    builder = builderFigure;
                    break;
                }
            }

            _model = builder.GameplayModel;
            _model.GameOver += GameOver;
            view.Ball.Collision += OnBallCollision;
            _ballCollisionCount = 0;

            _tutorialController.RingController = _ringController;
            _tutorialController.BallView = view.Ball;
            _tutorialController.GameplayModel = _model;
            _tutorialController.GameWindowController = this;

            RingInput.Instance.Press -= OnRingInputPress;
            RingInput.Instance.Press += OnRingInputPress;
            view.ShowStartText();
            _readyToPlay = true;

            _tutorialController.CheckTutorByStart();

#if DEV
            view.SetDebugWinButtonEnabled(!_isTutorial);
#endif
        }

        protected override void OnSubscribe()
        {
            view.MenuButton += ViewOnMenuButton;
            view.RestartButton += ViewOnRestartButton;

            if (_isTutorial == false)
            {
                _adsController.ShowBanner();
            }

            if (_model.Started)
            {
                view.Ball.ResumeBall();
            }

#if DEV
            view.DebugWinButton += OnDebugWinPress;
#endif

            base.OnSubscribe();
        }

        protected override void OnUnSubscribe()
        {
            view.MenuButton -= ViewOnMenuButton;
            view.RestartButton -= ViewOnRestartButton;

            _adsController.HideBanner();
            if (_model.Started)
            {
                view.Ball.Stop();
            }

#if DEV
            view.DebugWinButton -= OnDebugWinPress;
#endif

            base.OnUnSubscribe();
        }

        public override void Show(bool push, bool animation = true, Action animationFinished = null)
        {
            base.Show(push, animation, animationFinished);
            Input.multiTouchEnabled = false;
        }

        public override void Close(bool pop, bool animation = true, Action animationFinished = null)
        {
            base.Close(pop, animation, animationFinished);
            Input.multiTouchEnabled = true;
        }

        public void GameOver(EGameResult result)
        {
            StopGame();

            Action action;
            if (result == EGameResult.Win)
            {
                if (_isTutorial)
                {
                    _tutorialController.SaveTutorial();

                    action = Restart;
                }
                else
                {
                    if (_dataProvider.CurrentLevel < _levelProvider.GetLength())
                    {
                        _dataProvider.CurrentLevel++;
                    }

                    action = () =>
                    {
#if DEV
                        if (_skipAds)
                        {
                            _skipAds = false;
                            Restart();
                            return;
                        }
#endif
                        var adsController = ProjectContext.GetInstance<IAdsController>();
                        adsController.ShowInterstitialOnLevel(_model.LevelId, "game_finish", _ => Restart());
                    };
                }
            }
            else
            {
                action = Restart;
            }

            TrackProgressionEvent(result == EGameResult.Win
                ? GAProgressionStatus.Complete
                : GAProgressionStatus.Fail);

            var settings = new FinishWindowSettings
            {
                LevelId = _model.LevelId,
                RestartAction = action,
                IsTutorial = _isTutorial,
                SkipLevelAction = () =>
                {
                    if (_isTutorial)
                    {
                        _tutorialController.SaveTutorial();
                    }
                    else
                    {
                        if (_dataProvider.CurrentLevel < _levelProvider.GetLength())
                        {
                            _dataProvider.CurrentLevel++;
                        }
                    }

                    Restart();
                    uiManager.Back();
                },
                Result = result
            };
            uiManager.Open<FinishWindowController, FinishWindowSettings>(settings);
#if DEV_LOG
            Debug.Log(result);
#endif
        }

        public void Restart()
        {
            StopGame();
            InitLevel();
        }

        private void StopGame()
        {
            if (_model != null)
            {
                _model.Started = false;
            }

            if (view.Ball != null)
            {
                view.Ball.Stop();
            }
        }

        private void ReleaseLevel()
        {
            if (_model != null)
            {
                _model.GameOver -= GameOver;
                _model = null;
            }

            view.ReleaseLevel();
        }

        private void ViewOnMenuButton()
        {
            _adsController.HideBanner();
            uiManager.Open<MenuPopupWindowController>(true);
        }

        private void ViewOnRestartButton()
        {
            if (!_readyToPlay)
            {
                TrackProgressionEvent(GAProgressionStatus.Fail);

                Restart();

                if (!_isTutorial)
                {
                    var adsController = ProjectContext.GetInstance<IAdsController>();
                    adsController.ShowInterstitialOnLevel(_model.LevelId, "restart");
                }
            }
        }

        private void OnRingInputPress()
        {
            if (!_model.Started)
            {
                view.HideStartText();
                view.Ball.StartBall(view.Ring.InnerRadius);
                _model.Started = true;
                _readyToPlay = false;
                RingInput.Instance.Press -= OnRingInputPress;

                TrackProgressionEvent(GAProgressionStatus.Start);
            }
        }

        private void OnBallCollision(Vector2 point)
        {
            _ballCollisionCount++;
            if (_isTutorial)
            {
                _tutorialController.CheckTutorByBallCollision(_ballCollisionCount);
            }

            _ringController.OnBallCollision(point);

            var applicationUtils = NativeUtilsProvider.ApplicationUtils;
            var vibrationSwitch = ProjectContext.GetInstance<IPuDataProvider>().Vibration;
            if (applicationUtils.HasVibrator() && vibrationSwitch)
            {
                applicationUtils.Vibrate(30);
            }
        }

        private void TrackProgressionEvent(GAProgressionStatus status)
        {
            if (_isTutorial)
            {
                GameAnalytics.NewProgressionEvent(status, "Tutorial", _model.LevelId.ToString());
            }
            else
            {
                GameAnalytics.NewProgressionEvent(status, _model.LevelId.ToString());
            }
        }

#if DEV
        private bool _skipAds;

        private void OnDebugWinPress(bool longPress)
        {
            var dataProvider = ProjectContext.GetInstance<IPuDataProvider>();

            if (!longPress)
            {
                if (_model.Started)
                {
                    _skipAds = true;
                    _model.DebugWin();
                    return;
                }

                if (dataProvider.CurrentLevel < _levelProvider.GetLength())
                {
                    dataProvider.CurrentLevel++;
                }
            }
            else
            {
                dataProvider.AllLevelsPassedPopupShownAt = -1;

                if (dataProvider.CurrentLevel == 0)
                {
                    dataProvider.CurrentLevel = _levelProvider.GetLength() - 2;
                }
                else
                {
                    dataProvider.CurrentLevel = 0;
                }
            }

            InitLevel();
        }
#endif
    }
}