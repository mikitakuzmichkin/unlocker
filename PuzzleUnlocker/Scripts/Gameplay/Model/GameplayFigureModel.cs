using System.Linq;
using PuzzleUnlocker.Scripts.Extensions;

namespace PuzzleUnlocker.Gameplay.Model
{
    public class GameplayFigureModel : AGameplayModel<Figure, int?>
    {
        public override void Init<TLevelData>(TLevelData levelData)
        {
            SelectIndexes = levelData.SelectedIndices;
            if (SelectIndexes == null || SelectIndexes.Length < 1)
            {
                TargetCount = levelData.TargetsCount;
            }
            else
            {
                TargetCount = SelectIndexes.Length;
            }

            GetTarget = levelData.GetTarget;
            
            base.Init(levelData);
        }

        public override void BallConnectWithSegment(int segmentInd)
        {
            var segmentFigure = SegmentsValues[segmentInd];
            var segmentFigureIndex = segmentFigure.GetCurrentIndex();

            if (BallValue != null)
            {
                var ballValue = BallValue.Value;
                if (segmentFigureIndex.LoopShortDistance(ballValue, 0, segmentFigure.Max) != 1)
                {
                    GameOverResult(EGameResult.Lose);
                    return;
                }

                segmentFigure.ToIndex(ballValue);
            }

            BallValue = segmentFigureIndex;

            base.BallConnectWithSegment(segmentInd);

            if (SelectIndexes == null || SelectIndexes.Length < 1)
            {
                var selected = SegmentsValues.GetIndexes(v => v.FullCircle());
                if (selected.Length >= TargetCount)
                {
                    GameOverResult(EGameResult.Win);
                }
            }
            else
            {
                var selected = SelectIndexes.Where(i => SegmentsValues[i].FullCircle()).ToArray();
                LightSelect(selected);
                if (selected.Length == SelectIndexes.Length)
                {
                    GameOverResult(EGameResult.Win);
                }
            }
        }
    }
}