using System;
using PuzzleUnlocker.Gameplay;

namespace PuzzleUnlocker.UI.Windows
{
    public class FinishWindowSettings
    {
        public int LevelId;
        public Action RestartAction;
        public Action SkipLevelAction;
        public bool IsTutorial; 
        public EGameResult Result;
    }
}