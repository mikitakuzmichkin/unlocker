using System;
using System.Linq;
using PuzzleUnlocker.Scripts.Extensions;

namespace PuzzleUnlocker.Gameplay.Model
{
    public class GameplayCubeModel : AGameplayModel<int, int?>
    {
        private Func<object, bool> _checker;

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

            _checker = levelData.Checker;
            GetTarget = levelData.GetTarget;

            base.Init(levelData);
        }

        public override void BallConnectWithSegment(int segmentInd)
        {
            if (BallValue == null)
            {
                BallValue = 0;
            }

            if (SegmentsValues[segmentInd] + BallValue.Value == 12)
            {
                GameOverResult(EGameResult.Lose);
                return;
            }

            var sum = (SegmentsValues[segmentInd] + BallValue.Value) % 6;
            SegmentsValues[segmentInd] = sum == 0 ? 6 : sum;

            BallValue = SegmentsValues[segmentInd];

            base.BallConnectWithSegment(segmentInd);

            if (SelectIndexes == null || SelectIndexes.Length < 1)
            {
                var selected = SegmentsValues.GetIndexes(v => _checker(v));
                LightSelect(selected);
                if (selected.Length >= TargetCount)
                {
                    GameOverResult(EGameResult.Win);
                }
            }
            else
            {
                var selected = SelectIndexes.Where(i => _checker(SegmentsValues[i])).ToArray();
                LightSelect(selected);
                if (selected.Length == SelectIndexes.Length)
                {
                    GameOverResult(EGameResult.Win);
                }
            }
        }
    }
}