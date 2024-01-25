namespace PuzzleUnlocker.Gameplay.Level
{
    public interface ILevelDataProvider
    {
        int GetLength();
        ALevelData GetLevelData(int level);
    }
}