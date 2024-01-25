using System;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PuzzleUnlocker.Gameplay.Level
{
    [Serializable]
    public class LevelDataFigure : ALevelData
    {
        [JsonProperty] private FigureLevelData[] _array;
        [NonSerialized] private int _max;

        public LevelDataFigure(int id) : base(id)
        {
        }

        public void SetDataFromJson(string data)
        {
            _array = JsonConvert.DeserializeObject<FigureLevelData[]>(data);
            _max = _array.First().MaxInd;
        }

        public override T[] GetData<T>()
        {
            return GetData().Select(el => (T) el).ToArray();
        }

        public override object[] GetData()
        {
            return _array.Select(el => (object) el.ToFigure()).ToArray();
        }

        public override object GetTarget()
        {
            _max = _array.First().MaxInd;
            return new Figure(0,_max, _max);
        }

        public override bool Checker(object o)
        {
            throw new NotImplementedException();
        }

#if UNITY_EDITOR
        public override void DrawSegmentData()
        {
            var length = _array?.Length ?? 0;
            length = EditorGUILayout.IntField("Size", length);
            if (_array == null)
            {
                _array = new FigureLevelData[length];
            }
            else
            {
                if (_array.Length != length)
                {
                    Array.Resize(ref _array, length);
                }
            }

            if (_max == 0 && length > 0)
            {
                try
                {
                    _max = _array[0].MaxInd;
                }
                catch (NullReferenceException ex)
                {
                    EditorGUILayout.HelpBox("Forgot set max points", MessageType.Error);
                    return;
                }
            }
            _max = EditorGUILayout.IntField("MaxPoints", _max);
            
            for (var i = 0; i < _array.Length; i++)
            {
                if (_array[i] == null)
                {
                    _array[i] = new FigureLevelData();
                }

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Elem " + i);
                _array[i].StartInd = EditorGUILayout.IntField("Start", _array[i].StartInd);
                _array[i].LinesCount = EditorGUILayout.IntField("Lines count", _array[i].LinesCount);
                GUILayout.EndHorizontal();

                _array[i].MaxInd = _max;

                if (_array[i].StartInd > _max)
                {
                    EditorGUILayout.HelpBox("Max value exception", MessageType.Error);
                }
            }
        }
#endif

        [Serializable]
        private class FigureLevelData
        {
            public int StartInd;
            public int LinesCount;
            public int MaxInd;

            public Figure ToFigure()
            {
                return new Figure(StartInd, LinesCount, MaxInd);
            }
        }
    }
}