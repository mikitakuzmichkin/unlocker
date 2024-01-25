using PuzzleUnlocker.Tutorials;
using PuzzleUnlocker.UI.Windows;
using UnityEngine;

namespace PuzzleUnlocker.SceneControllers
{
    public class GameplaySceneController : SceneControllerBase
    {
        protected override void Initialize()
        {
            uiManager.Open<GameWindowController>();
        }
    }
}