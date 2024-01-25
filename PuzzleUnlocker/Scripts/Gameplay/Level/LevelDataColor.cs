using System;
using System.Linq;
using Newtonsoft.Json;
using PuzzleUnlocker.Scripts.Extensions;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace PuzzleUnlocker.Gameplay.Level
{
    [Serializable]
    public class LevelDataColor : ALevelData
    {
        [JsonProperty] private float[][] _floatsArray;
        [JsonProperty] private float[] _target;

        public LevelDataColor(int id) : base(id)
        {
        }

        public void SetDataFromJson(string data)
        {
            _floatsArray = JsonConvert.DeserializeObject<float[][]>(data);
        }

        public override T[] GetData<T>()
        {
            return GetData().Select(el => (T) el).ToArray();
        }

        public override object[] GetData()
        {
            return _floatsArray.Select(el => (object) el.FloatToColor()).ToArray();
        }

        public override object GetTarget()
        {
            return _target.FloatToColor();
        }

        public override bool Checker(object o)
        {
            return (Color) o == _target.FloatToColor();
        }

#if UNITY_EDITOR
        public override void DrawSegmentData()
        {
            _target = EditorGUILayout.ColorField("Target", _target.FloatToColor()).ColorToFloat();

            var length = _floatsArray?.Length ?? 0;
            length = EditorGUILayout.IntField("Size", length);
            if (_floatsArray == null)
            {
                _floatsArray = new float[length][];
            }
            else
            {
                if (_floatsArray.Length != length)
                {
                    Array.Resize(ref _floatsArray, length);
                }
            }

            for (var i = 0; i < _floatsArray.Length; i++)
            {
                _floatsArray[i] = EditorGUILayout.ColorField("Elem " + i, _floatsArray[i].FloatToColor())
                    .ColorToFloat();
            }
        }
#endif
    }
}