using System.Linq;
using PuzzleUnlocker.Gameplay.Content;
using PuzzleUnlocker.Gameplay.Level;
using PuzzleUnlocker.Gameplay.Model;
using PuzzleUnlocker.Gameplay.Ring;
using PuzzleUnlocker.Scripts.Gameplay.Ball;
using PuzzleUnlocker.UI.Windows;
using UnityEngine;

namespace PuzzleUnlocker.Gameplay.Builder
{
    public class GameplayFigureBuilder : GameplayBuilder
    {
        public void Build(GameWindowView view, ALevelData level, out RingController<Figure, int?> ringController)
        {
            var model = new GameplayFigureModel();
            model.Init(level);

            var pointCount = level.GetData<Figure>().First().Max;
            AGameplayContent content;
            BallView ball;
            GameObject targetContent;
            if (pointCount == 3)
            {
                content = Resources.Load<GameObject>("Content/ContentFigureTriangle").GetComponent<AGameplayContent>();
                ball = Resources.Load<GameObject>("Ball/BallFigureTriangle").GetComponent<BallView>();
                targetContent = Resources.Load<GameObject>("Target/TargetFigureTriangle");
            }
            else
            {
                content = Resources.Load<GameObject>("Content/ContentFigureCube").GetComponent<AGameplayContent>();
                ball = Resources.Load<GameObject>("Ball/BallFigureCube").GetComponent<BallView>();
                targetContent = Resources.Load<GameObject>("Target/TargetFigureCube");
            }

            Build(view, null, content, model.SegmentsCount, model, ball, out ringController, targetContent);
        }
    }
}