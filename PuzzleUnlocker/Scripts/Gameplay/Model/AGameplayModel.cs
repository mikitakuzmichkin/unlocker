using System;
using System.Collections;
using PuzzleUnlocker.Gameplay.Level;

namespace PuzzleUnlocker.Gameplay.Model
{
    public abstract class AGameplayModel<TSegmentValue, TBallValue> : IGameplayModel
    {
        protected TBallValue BallValue;
        protected TSegmentValue[] SegmentsValues;
        protected BitArray LightSelectedSegments;
        protected Func<object> GetTarget;

        public int LevelId { get; private set; }

        public int[] SelectIndexes { get; protected set; }
        public int TargetCount { get; protected set; }
        public bool Started { get; set; }
        public int SegmentsCount => SegmentsValues.Length;
        public object Target => GetTarget();

        public event Action<TSegmentValue[]> UpdatedSegments;
        public event Action<BitArray> UpdatedSelectedLightSegments;
        public event Action<object> UpdateBallValue;
        public event Action<EGameResult> GameOver;
        public object GetSegmentValue(int index)
        {
            return SegmentsValues[index];
        }

        public object GetBallValue()
        {
            return BallValue;
        }

        public virtual void Init<TLevelData>(TLevelData levelData)
            where TLevelData : ALevelData
        {
            LevelId = levelData.Id;
            SegmentsValues = levelData.GetData<TSegmentValue>();
            LightSelectedSegments = new BitArray(SegmentsValues.Length);
            UpdatedSegments?.Invoke(SegmentsValues);
        }

        public void ForceSetBallValue(object ballValue)
        {
            BallValue = (TBallValue)ballValue;
            UpdateBallValue?.Invoke(BallValue);
        }

        public virtual void BallConnectWithSegment(int segmentInd)
        {
            UpdateBallValue?.Invoke(BallValue);
            UpdatedSegments?.Invoke(SegmentsValues);
        }

        public void UpdateSegments()
        {
            UpdateBallValue?.Invoke(BallValue);
            UpdatedSegments?.Invoke(SegmentsValues);
        }

        protected void GameOverResult(EGameResult result)
        {
            GameOver?.Invoke(result);
        }

        protected void LightSelect(int[] indexes)
        {
            LightSelectedSegments.SetAll(false);

            var ind = 0;
            var indexesCount = indexes.Length;
            if (indexesCount > 0)
            {
                var index = indexes[ind];
                for (var i = 0; i < LightSelectedSegments.Length; i++)
                {
                    if (i != index)
                    {
                        continue;
                    }

                    LightSelectedSegments[i] = true;
                    if (ind < indexesCount - 1)
                    {
                        ind++;
                        index = indexes[ind];
                    }
                }
            }

            UpdatedSelectedLightSegments?.Invoke(LightSelectedSegments);
        }

#if DEV
        public void DebugWin()
        {
            GameOverResult(EGameResult.Win);
        }
#endif
    }
}