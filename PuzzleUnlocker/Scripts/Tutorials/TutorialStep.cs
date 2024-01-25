using System;
using PuzzleUnlocker.Gameplay.Level;

namespace PuzzleUnlocker.Tutorials
{
    [Serializable]
    public class TutorialStep
    {
        public int Id;
        public int? ShowBeforeLevel;
        public ALevelData LevelData;
    }
}