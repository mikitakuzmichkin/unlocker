using System.Threading.Tasks;
using Dainty.Ads;
using Dainty.DI;
using GameAnalyticsSDK;
using PuzzleUnlocker.Data;
using PuzzleUnlocker.Gameplay.Level;
using PuzzleUnlocker.Services;
using PuzzleUnlocker.Services.Ads;
using PuzzleUnlocker.Tutorials;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PuzzleUnlocker.SceneControllers
{
    public class BootstrapSceneController : SceneControllerBase
    {
        [SerializeField] private LevelDataJsonProvider _levelProvider;
        [SerializeField] private TutorialDataJsonProvider _tutorialProvider;

        protected override void Initialize()
        {
            DontDestroyOnLoad(_levelProvider);
            ProjectContext.Bind<ILevelDataProvider>(_levelProvider);
            ProjectContext.Bind<ITutorialDataProvider>(_tutorialProvider);

            IPuDataProvider dataProvider = new PuDataProvider();
            dataProvider.Initialize();
            ProjectContext.Bind<IPuDataProvider>(dataProvider);

#if UNITY_EDITOR
            var adsObj = Instantiate(new GameObject("[Editor Ads]"));
            DontDestroyOnLoad(adsObj);
            IAdsController adsController = adsObj.AddComponent<PuEditorAdsController>();
#else
            IAdsController adsController = new PuAdsController();
#endif
            adsController.Initialize(true);
#if MOTION_DESIGN
            adsController.Enabled = false;
#endif
            ProjectContext.Bind(adsController);

            InitGameAnalytics();

            var asyncOperation = SceneManager.LoadSceneAsync("Game");
            asyncOperation.allowSceneActivation = false;
            PuFacebookManager.InitAsync()
                .ContinueWith(task => asyncOperation.allowSceneActivation = true,
                    TaskScheduler.FromCurrentSynchronizationContext()
                );
        }

        private static void InitGameAnalytics()
        {
            GameAnalytics.SettingsGA.InfoLogBuild =
#if DEV_LOG
                true;
#else
                false;
#endif

#if UNITY_IOS
            GameAnalytics.RequestTrackingAuthorization(this);
#else
            GameAnalytics.Initialize();
#endif
        }
    }
}