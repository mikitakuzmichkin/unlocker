using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace PuzzleUnlocker.Gameplay.Level.Editor
{
    public class LevelEditorWindow : EditorWindow
    {
        private string _path;
        private List<LevelEditor> _levels;
        private int _select = -1;
        Vector2 _scrollPos;

        [MenuItem("PuzzleUnLocker/Level Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<LevelEditorWindow>();
            window.titleContent = new GUIContent("Level Editor");
            window.Show();
        }

        private void OnGUI()
        {
            if (_path != null)
            {
                EditorGUILayout.LabelField(_path);
            }

            if (_path == null)
            {
                if (GUILayout.Button("Create New"))
                {
                    CreateNew();
                }
            }
            else
            {
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
                DrawLevels();
                EditorGUILayout.EndScrollView();

                if (_select >= 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    if (_select > 0)
                    {
                        if (GUILayout.Button("^"))
                        {
                            var temp = _levels[_select - 1];
                            _levels[_select - 1] = _levels[_select];
                            _levels[_select] = temp;
                            _select -= 1;
                        }
                    }

                    if (_select < _levels.Count - 1)
                    {
                        if (GUILayout.Button("v"))
                        {
                            var temp = _levels[_select + 1];
                            _levels[_select + 1] = _levels[_select];
                            _levels[_select] = temp;
                            _select += 1;
                        }
                    }

                    if (GUILayout.Button("+"))
                    {
                        _levels.Insert(_select, new LevelEditor());
                    }

                    if (GUILayout.Button("-"))
                    {
                        _levels.RemoveAt(_select);
                        _select = -1;
                    }

                    EditorGUILayout.EndHorizontal();
                }

                if (GUILayout.Button("ReLoad"))
                {
                    Load();
                }

                if (GUILayout.Button("Save"))
                {
                    var isCompact = EditorUtility.DisplayDialog("Target", "", "Release (Compact)", "Debug (Beauty)");
                    var text = SerializeLevels(isCompact ? Formatting.None : Formatting.Indented);
                    File.WriteAllText(_path, text);
                }

                if (GUILayout.Button("Save As"))
                {
                    var isCompact = EditorUtility.DisplayDialog("Target", "", "Release (Compact)", "Debug (Beauty)");
                    var text = SerializeLevels(isCompact ? Formatting.None : Formatting.Indented);
                    CreateNew(text);
                }
            }

            if (GUILayout.Button("Open"))
            {
                Open();
                Load();
            }
        }

        private string SerializeLevels(Formatting formatting)
        {
            var elems = _levels.Select(t => t.Data).ToList();
            for (var i = 0; i < elems.Count; i++)
            {
                elems[i].Id = i;
            }

            return JsonConvert.SerializeObject(elems, formatting,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }

        private void Load()
        {
            string text = "";
            using (var sr = new StreamReader(_path))
            {
                text = sr.ReadToEnd();
            }

            var levels = JsonConvert.DeserializeObject<ALevelData[]>(text,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            _levels = levels
                .Select(l => new LevelEditor { Type = l.GameType, Data = l })
                .ToList();
        }

        private void CreateNew(string text = "")
        {
            var path = EditorUtility.SaveFilePanel("level", "Assets/PuzzleUnlocker/Data", "levels", "json");

            if (path.Length == 0)
                return;

            using (var sw = File.CreateText(path))
            {
                sw.Write(text);
            }

            _path = path;
        }

        private void Open()
        {
            _path = EditorUtility.OpenFilePanel("level", "Assets/PuzzleUnlocker/Data", "json");
        }

        private void DrawLevels()
        {
            int length = _levels?.Count ?? 0;

            if (_levels == null)
            {
                _levels = new List<LevelEditor>();
            }
            else
            {
                if (_levels.Count != length)
                {
                    while (_levels.Count > length)
                    {
                        _levels.RemoveAt(_levels.Count - 1);
                    }

                    while (_levels.Count < length)
                    {
                        _levels.Add(new LevelEditor());
                    }
                }
            }

            for (int i = 0; i < _levels.Count; i++)
            {
                UnityEditor.EditorGUILayout.BeginHorizontal();
                Rect rt = GUILayoutUtility.GetRect(20, 20, 10, 200);
                if (_select == i)
                {
                    GUI.color = Color.cyan;
                }

                if (GUI.Button(rt, ""))
                {
                    if (_select == i)
                    {
                        _select = -1;
                    }
                    else
                    {
                        _select = i;
                    }
                }

                GUI.color = Color.white;
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Level " + i);

                if (_levels[i] == null)
                {
                    _levels[i] = new LevelEditor();
                }

                _levels[i].Type = (EGameType) EditorGUILayout.EnumPopup(_levels[i].Type);

                if (_levels[i].Data == null || _levels[i].Data.GameType != _levels[i].Type)
                {
                    var newData = ALevelData.GetLevelData(i, _levels[i].Type);

                    var oldData = _levels[i].Data;
                    if (oldData != null)
                    {
                        newData.SelectedIndices = oldData.SelectedIndices;
                        newData.TargetsCount = oldData.TargetsCount;

                        if (oldData is LevelDataInteger && newData is LevelDataInteger)
                        {
                            var type = typeof(LevelDataInteger);
                            CopyFields(type, oldData, newData, "_array", "_target", "_targetType");
                        }
                    }

                    _levels[i].Data = newData;
                }

                _levels[i].Show = EditorGUILayout.Foldout(_levels[i].Show, "level data");
                if (_levels[i].Show)
                {
                    _levels[i].Data.DrawLevelData();
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(20f);
            }

            EditorGUILayout.Space(20f);
        }

        private static void CopyField(Type type, object from, object to, string fieldName)
        {
            var fieldInfo = type.GetField(
                fieldName,
                BindingFlags.Instance
                | BindingFlags.NonPublic
                | BindingFlags.Public
            );
            fieldInfo.SetValue(to, fieldInfo.GetValue(from));
        }

        private static void CopyFields(Type type, object from, object to, params string[] fieldNames)
        {
            foreach (var fieldName in fieldNames)
            {
                CopyField(type, from, to, fieldName);
            }
        }

        private class LevelEditor
        {
            public EGameType Type;
            public ALevelData Data;
            public bool Show;
        }
    }
}