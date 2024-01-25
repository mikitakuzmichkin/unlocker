using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PuzzleUnlocker.Gameplay;
using PuzzleUnlocker.Gameplay.Level;
using UnityEditor;
using UnityEngine;

namespace PuzzleUnlocker.Tutorials.Editor
{
    public class TutorialEditorWindow : EditorWindow
    {
        private string _path;
        private List<TutorialEditor> _tutorials;
        Vector2 _scrollPos;

        [MenuItem("PuzzleUnLocker/Tutorial Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<TutorialEditorWindow>();
            window.titleContent = new GUIContent("Tutorial Editor");
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
            var elems = _tutorials.Select(t => t.Data).ToList();
            return JsonConvert.SerializeObject(elems, formatting,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        }

        private void Load()
        {
            string text;
            using (var sr = new StreamReader(_path))
            {
                text = sr.ReadToEnd();
            }

            var levels = JsonConvert.DeserializeObject<TutorialData[]>(text,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            _tutorials = levels
                .Select(t => new TutorialEditor { Data = t })
                .ToList();
        }

        private void CreateNew(string text = "")
        {
            var path = EditorUtility.SaveFilePanel("levelTutorial", "Assets/PuzzleUnlocker/Data", "levelTutorial",
                "json");

            if (path.Length == 0)
            {
                return;
            }

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
            if (_tutorials == null)
            {
                _tutorials = new List<TutorialEditor>();
                foreach (var gameType in Enum.GetValues(typeof(EGameType)))
                {
                    var tutorialData = new TutorialData { Stage = (EGameType) gameType };
                    _tutorials.Add(new TutorialEditor { Data = tutorialData });
                }
            }

            foreach (var tutorial in _tutorials)
            {
                var name = Enum.GetName(typeof(EGameType), tutorial.Data.Stage);
                tutorial.Show = EditorGUILayout.Foldout(tutorial.Show, name);
                if (tutorial.Show)
                {
                    var length = tutorial.Data.TutorialSteps?.Count ?? 0;
                    length = EditorGUILayout.IntField("size", length);

                    if (tutorial.Data.TutorialSteps == null)
                    {
                        tutorial.Data.TutorialSteps = new List<TutorialStep>();
                    }
                    else
                    {
                        if (tutorial.Data.TutorialSteps.Count != length)
                        {
                            while (tutorial.Data.TutorialSteps.Count > length)
                            {
                                tutorial.Data.TutorialSteps.RemoveAt(tutorial.Data.TutorialSteps.Count - 1);
                            }

                            while (tutorial.Data.TutorialSteps.Count < length)
                            {
                                tutorial.Data.TutorialSteps.Add(new TutorialStep { Id = tutorial.Data.TutorialSteps.Count });
                            }
                        }
                    }

                    foreach (var step in tutorial.Data.TutorialSteps)
                    {
                        EditorGUILayout.LabelField("id " + step.Id);
                        var hasLevelStart = step.ShowBeforeLevel != null;
                        hasLevelStart = EditorGUILayout.Toggle("Show Before Level", hasLevelStart);
                        if (hasLevelStart)
                        {
                            if (step.ShowBeforeLevel == null)
                            {
                                step.ShowBeforeLevel = 0;
                            }

                            step.ShowBeforeLevel = EditorGUILayout.IntField("Level", (int) step.ShowBeforeLevel);
                        }
                        else
                        {
                            step.ShowBeforeLevel = null;
                        }

                        var hasLevel = step.LevelData != null;
                        hasLevel = EditorGUILayout.Toggle("Show Level", hasLevel);
                        if (hasLevel)
                        {
                            if (step.LevelData == null)
                            {
                                step.LevelData = ALevelData.GetLevelData(step.Id, tutorial.Data.Stage);
                            }

                            step.LevelData.DrawLevelData();
                        }
                        else
                        {
                            step.LevelData = null;
                        }

                        EditorGUILayout.Space(20f);
                    }
                }
            }
        }

        [Serializable]
        private class TutorialEditor
        {
            public TutorialData Data;
            public bool Show;
        }
    }
}