using Newtonsoft.Json;
using UnityEngine;

namespace PuzzleUnlocker.Gameplay.Level
{
    public class LevelDataJsonProvider : MonoBehaviour, ILevelDataProvider
    {
        [SerializeField] private TextAsset _json;

        private ALevelData[] _levelsData;

        private void Awake()
        {
            _levelsData = JsonConvert.DeserializeObject<ALevelData[]>(_json.text,
                new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.Auto});
        }

        public int GetLength()
        {
            return _levelsData.Length;
        }

        public ALevelData GetLevelData(int level)
        {
            return _levelsData[level];
        }
    }
}