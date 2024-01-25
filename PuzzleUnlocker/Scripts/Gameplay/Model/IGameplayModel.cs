using System;

namespace PuzzleUnlocker.Gameplay.Model
{
    public interface IGameplayModel
    {
        int LevelId { get; }
        public bool Started { get; set; }

        public event Action<object> UpdateBallValue;
        public event Action<EGameResult> GameOver;

        public object GetSegmentValue(int index);
        public object GetBallValue();
        void ForceSetBallValue(object ballValue);

#if DEV
        void DebugWin();
#endif
    }
}