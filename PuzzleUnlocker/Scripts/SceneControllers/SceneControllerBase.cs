using Dainty.UI;
using UnityEngine;

namespace PuzzleUnlocker.SceneControllers
{
    [DefaultExecutionOrder(-30)]
    public abstract class SceneControllerBase : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] protected UiRoot uiRoot;
        [SerializeField] protected UiManagerSettings uiManagerSettings;
#pragma warning restore 649

        protected UiManager uiManager;

        protected virtual void Awake()
        {
            if (uiRoot != null && uiManagerSettings != null)
            {
                uiManager = new UiManager(uiRoot, uiManagerSettings);
            }
        }

        protected virtual void Start()
        {
            Initialize();
        }

        protected abstract void Initialize();
    }
}