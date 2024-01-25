using System;
using Newtonsoft.Json;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace PuzzleUnlocker.Gameplay.Level
{
    [Serializable]
    public abstract class ALevelData
    {
        [JsonProperty("id")] private int _id;
        [JsonProperty("type")] private EGameType _gameType;
        [JsonProperty("selected_indices")] public int[] SelectedIndices;
        [JsonProperty("targets_count")] public int TargetsCount;

        [NonSerialized] private bool _showSegmentData;
        [NonSerialized] private bool _showSelectedIndexes;

        [JsonIgnore]
        public int Id
        {
            get => _id;
            set => _id = value;
        }

        [JsonIgnore]
        public EGameType GameType
        {
            get => _gameType;
            private set => _gameType = value;
        }

        /*protected ALevelData()
        {
        }*/

        protected ALevelData(int id)
        {
            _id = id;
        }

        public static ALevelData GetLevelData(int id, EGameType gameType)
        {
            ALevelData levelData = null;
            switch (gameType)
            {
                case EGameType.Cube:
                case EGameType.Number:
                    levelData = new LevelDataInteger(id);
                    break;
                case EGameType.Color:
                    levelData = new LevelDataColor(id);
                    break;
                case EGameType.Figure:
                    levelData = new LevelDataFigure(id);
                    break;
            }

            levelData.GameType = gameType;
            return levelData;
        }

        public static ALevelData GetLevelData(EGameType gameType, string json)
        {
            ALevelData levelData = null;
            switch (gameType)
            {
                case EGameType.Cube:
                case EGameType.Number:
                    levelData = JsonConvert.DeserializeObject<LevelDataInteger>(json);
                    break;
                case EGameType.Color:
                    levelData = JsonConvert.DeserializeObject<LevelDataColor>(json);
                    break;
                case EGameType.Figure:
                    levelData = JsonConvert.DeserializeObject<LevelDataFigure>(json);
                    break;
            }

            levelData.GameType = gameType;
            return levelData;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public abstract T[] GetData<T>();

        public abstract object[] GetData();

        public abstract object GetTarget();

        public abstract bool Checker(object o);

#if UNITY_EDITOR
        public void DrawLevelData()
        {
            _showSelectedIndexes = EditorGUILayout.Foldout(_showSelectedIndexes, "Select Indexes");
            if (_showSelectedIndexes)
            {
                var length = SelectedIndices?.Length ?? 0;
                length = EditorGUILayout.IntField("Size", length);
                if (SelectedIndices == null)
                {
                    SelectedIndices = new int[length];
                }
                else
                {
                    if (SelectedIndices.Length != length)
                    {
                        Array.Resize(ref SelectedIndices, length);
                    }
                }

                for (var i = 0; i < SelectedIndices.Length; i++)
                {
                    SelectedIndices[i] = EditorGUILayout.IntField("Elem " + i, SelectedIndices[i]);
                }
            }

            TargetsCount = EditorGUILayout.IntField("TargetCount", TargetsCount);

            _showSegmentData = EditorGUILayout.Foldout(_showSegmentData, "segment data");
            if (_showSegmentData)
            {
                DrawSegmentData();
            }
        }

        public abstract void DrawSegmentData();
#endif
    }
}