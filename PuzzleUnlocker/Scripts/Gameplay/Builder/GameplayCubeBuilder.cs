using PuzzleUnlocker.Gameplay.Content;
using PuzzleUnlocker.Gameplay.Level;
using PuzzleUnlocker.Gameplay.Model;
using PuzzleUnlocker.Gameplay.Ring;
using PuzzleUnlocker.UI.Windows;
using UnityEngine;

namespace PuzzleUnlocker.Gameplay.Builder
{
    public class GameplayCubeBuilder : GameplayBuilder
    {
        public void Build(GameWindowView view, ALevelData level, out RingController<int, int?> ringController)
        {
            var model = new GameplayCubeModel();
            model.Init(level);

            var content = Resources.Load<GameObject>("Content/ContentCube").GetComponent<AGameplayContent>();
            var targetContent = Resources.Load<GameObject>("Target/TargetCube");

            Build(view, content, model, out ringController, targetContent);
        }
    }
}