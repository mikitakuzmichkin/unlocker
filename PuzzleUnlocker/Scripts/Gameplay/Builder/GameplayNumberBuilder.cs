using PuzzleUnlocker.Gameplay.Content;
using PuzzleUnlocker.Gameplay.Level;
using PuzzleUnlocker.Gameplay.Model;
using PuzzleUnlocker.Gameplay.Ring;
using PuzzleUnlocker.Gameplay.Segment;
using PuzzleUnlocker.Scripts.Gameplay.Ball;
using PuzzleUnlocker.UI.Windows;
using UnityEngine;

namespace PuzzleUnlocker.Gameplay.Builder
{
    public class GameplayNumberBuilder : GameplayBuilder
    {
        public void Build(GameWindowView view, ALevelData levelData, out RingController<int, int?> ringController)
        {
            var model = new GameplayNumberModel();
            model.Init(levelData);

            var ball = Resources.Load<GameObject>("Ball/BallNumber").GetComponent<BallView>();
            var segment = Resources.Load<GameObject>("Segment/SegmentNumber").GetComponent<SegmentView>();
            var targetContent = Resources.Load<GameObject>("Target/TargetNumber");

            Build(view, segment, null, model.SegmentsCount, model, ball, out ringController, targetContent);
        }
    }
}