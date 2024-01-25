using Dainty.Data;
using PuzzleUnlocker.Gameplay;

namespace PuzzleUnlocker.Data
{
    public interface IPuDataProvider : IDataProvider
    {
        int CurrentLevel { get; set; }
        
        bool Vibration { get; set; }
        int AllLevelsPassedPopupShownAt { get; set; }

        void SetTutorialLevelId(int id, EGameType stage);
        int GetTutorialLevelId(EGameType stage);
    }
}