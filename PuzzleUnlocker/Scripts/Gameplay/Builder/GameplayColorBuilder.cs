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
    public class GameplayColorBuilder : GameplayBuilder
    {
        public void Build(GameWindowView view, ALevelData level, out RingController<Color, Color> ringController)
        {
            var model = new GameplayColorModel();
            model.Init(level);

            var content = Resources.Load<GameObject>("Content/ContentColor").GetComponent<AGameplayContent>();
            var ball = Resources.Load<GameObject>("Ball/BallColor").GetComponent<BallView>();
            var targetContent = Resources.Load<GameObject>("Target/TargetColor");
            var segment = Resources.Load<GameObject>("Segment/SegmentColor").GetComponent<SegmentView>();

            Build(view, segment, content, model.SegmentsCount, model, ball, out ringController, targetContent);
        }
    }
}