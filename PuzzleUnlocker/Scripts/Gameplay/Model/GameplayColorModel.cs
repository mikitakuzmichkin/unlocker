using System;
using System.Linq;
using PuzzleUnlocker.Scripts.Extensions;
using PuzzleUnlocker.Scripts.Gameplay.Ball;
using TriangleNet.Voronoi.Legacy;
using UnityEngine;

namespace PuzzleUnlocker.Gameplay.Model
{
    public class GameplayColorModel : AGameplayModel<Color, Color>
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
            BallValue = Color.clear;

            base.Init(levelData);
        }

        public override void BallConnectWithSegment(int segmentInd)
        {
            if (SegmentsValues[segmentInd] == BallValue)
            {
                GameOverResult(EGameResult.Lose);
                return;
            }

            var segment = SegmentsValues[segmentInd];
            segment = segment.XorColor(BallValue);
            SegmentsValues[segmentInd] = segment;

            BallValue = segment;

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