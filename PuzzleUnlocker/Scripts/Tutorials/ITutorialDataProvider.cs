using System.Collections.Generic;
using PuzzleUnlocker.Gameplay;

namespace PuzzleUnlocker.Tutorials
{
    public interface ITutorialDataProvider
    {
        int GetLength();
        bool TryGetTutorialsData(int level, out List<TutorialStep> steps, out EGameType type);
        int GetLevelsBefore(int curLevel, bool isTutorial);
    }
}