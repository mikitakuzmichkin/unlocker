using System;
using Dainty.Data;
using Dainty.Data.Migration;
using PuzzleUnlocker.Gameplay;

namespace PuzzleUnlocker.Data
{
    public sealed class PuDataProvider : DataProviderBase, IPuDataProvider
    {
        private IStorageProvider _storageProvider;

        protected override void InitializeInternal()
        {
            _storageProvider = PlayerPrefsStorageProvider_v1.Instance;
        }

        protected override IMigrationTool GetMigrationTool()
        {
            return null;
        }

        #region IPuDataProvider

        public int CurrentLevel
        {
            get => _storageProvider.GetValue<int>(PrefKeys.CurrentLevelKey);
            set => SetAndFlush(_storageProvider, PrefKeys.CurrentLevelKey, value);
        }

        public bool Vibration
        {
            get => _storageProvider.GetValue<bool>(PrefKeys.VibrationKey, true);
            set => SetAndFlush(_storageProvider, PrefKeys.VibrationKey, value);
        }

        public int AllLevelsPassedPopupShownAt
        {
            get => _storageProvider.GetValue(PrefKeys.AllLevelsPassedPopupShownAtKey, -1);
            set => SetAndFlush(_storageProvider, PrefKeys.AllLevelsPassedPopupShownAtKey, value);
        }

        public void SetTutorialLevelId(int id, EGameType stage)
        {
            switch (stage)
            {
                case EGameType.Number:
                    SetAndFlush(_storageProvider, PrefKeys.TutorialIdForNumbers, id);
                    return;
                case EGameType.Cube:
                    SetAndFlush(_storageProvider, PrefKeys.TutorialIdForCube, id);
                    return;
                case EGameType.Color:
                    SetAndFlush(_storageProvider, PrefKeys.TutorialIdForColors, id);
                    return;
                case EGameType.Figure:
                    SetAndFlush(_storageProvider, PrefKeys.TutorialIdForFigures, id);
                    return;
            }
        }

        public int GetTutorialLevelId(EGameType stage)
        {
            switch (stage)
            {
                case EGameType.Number:
                    return _storageProvider.GetValue<int>(PrefKeys.TutorialIdForNumbers, -1);
                case EGameType.Cube:
                    return _storageProvider.GetValue<int>(PrefKeys.TutorialIdForCube, -1);
                case EGameType.Color:
                    return _storageProvider.GetValue<int>(PrefKeys.TutorialIdForColors, -1);
                case EGameType.Figure:
                    return _storageProvider.GetValue<int>(PrefKeys.TutorialIdForFigures, -1);
            }
            
            throw new ArgumentException();
        }

        private static void SetAndFlush<T>(IStorageProvider storageProvider, string key, T value)
        {
            storageProvider.SetValue(key, value);
            storageProvider.Flush();
        }

        #endregion
    }
}