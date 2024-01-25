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
    public class LevelDataInteger : ALevelData
    {
        [JsonProperty] private int[] _array;
        [JsonProperty] private int _target;
        [JsonProperty] private ETargetType _targetType;

        public LevelDataInteger(int id) : base(id)
        {
        }

        public void SetDataFromJson(string data)
        {
            _array = JsonConvert.DeserializeObject<int[]>(data);
        }

        public override T[] GetData<T>()
        {
            return GetData().Select(el => (T) el).ToArray();
        }

        public override object[] GetData()
        {
            return _array.Select(el => (object) el).ToArray();
        }

        public override object GetTarget()
        {
            switch (_targetType)
            {
                case ETargetType.Plus:
                    return "+";
                case ETargetType.Minus:
                    return "-";
            }

            return _target;
        }

        public override bool Checker(object o)
        {
            int number = (int) o;
            switch (_targetType)
            {
                case ETargetType.Plus:
                    return number > 0;
                case ETargetType.Minus:
                    return number < 0;
            }

            return number == _target;
        }

#if UNITY_EDITOR
        public override void DrawSegmentData()
        {
            _targetType = (ETargetType) EditorGUILayout.EnumPopup(_targetType);
            if (_targetType == ETargetType.Number)
            {
                _target = EditorGUILayout.IntField("Target", _target);
            }

            var length = _array?.Length ?? 0;
            length = EditorGUILayout.IntField("Size", length);
            if (_array == null)
            {
                _array = new int[length];
            }
            else
            {
                if (_array.Length != length)
                {
                    Array.Resize(ref _array, length);
                }
            }

            for (var i = 0; i < _array.Length; i++)
            {
                _array[i] = EditorGUILayout.IntField("Elem " + i, _array[i]);
            }
        }
#endif

        private enum ETargetType
        {
            Number,
            Plus,
            Minus
        }
    }
}