using PuzzleUnlocker.Gameplay.Content;
using PuzzleUnlocker.Gameplay.Model;
using PuzzleUnlocker.Gameplay.Ring;
using PuzzleUnlocker.Gameplay.Segment;
using PuzzleUnlocker.Scripts.Gameplay.Ball;
using PuzzleUnlocker.UI.Windows;
using UnityEngine;

namespace PuzzleUnlocker.Gameplay.Builder
{
    public class GameplayBuilder
    {
        public IGameplayModel GameplayModel { get; private set; }

        public void Build<T, TB>(GameWindowView view, SegmentView segmentView, AGameplayContent content,
            int segmentCount, AGameplayModel<T, TB> model, BallView ball, out RingController<T, TB> ringController, GameObject targetContent = null)
        {
            var ring = view.CreateRing(segmentView, content, segmentCount);

            var ballView = view.CreateBall(content, ball);
            ballView.Init(model);

            ringController = new RingController<T, TB>(ring, model);
            ringController.Subscribe();

            if (targetContent == null)
            {
                targetContent = content.gameObject;
            }
            view.CreateTarget(targetContent, model.Target, model.TargetCount);

            model.UpdateSegments();

            GameplayModel = model;
        }

        public void Build<T, TB>(GameWindowView view, AGameplayContent content, AGameplayModel<T, TB> model, out RingController<T, TB> ringController, GameObject targetContent = null)
        {
            Build(view, null, content, model.SegmentsCount, model, null, out ringController, targetContent);
        }
    }
}