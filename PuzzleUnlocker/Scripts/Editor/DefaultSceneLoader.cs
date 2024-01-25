using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

namespace PuzzleUnlocker.Editor
{
    [InitializeOnLoad]
    public static class DefaultSceneLoader
    {
        private static class ToolbarStyles
        {
            public static readonly GUIStyle commandButtonStyle;

            static ToolbarStyles()
            {
                commandButtonStyle = new GUIStyle("Command")
                {
                    fontSize = 16,
                    alignment = TextAnchor.MiddleCenter,
                    imagePosition = ImagePosition.ImageAbove,
                    fontStyle = FontStyle.Bold
                };
            }
        }

        static DefaultSceneLoader()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
        }

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(new GUIContent("1", "Start Scene 1"), ToolbarStyles.commandButtonStyle))
            {
                SetPlayModeStartScene("Assets/PuzzleUnlocker/Scenes/Bootstrap.unity");
                UnityEditor.EditorApplication.isPlaying = true;
            }

            // if (GUILayout.Button(new GUIContent("2", "Start Scene 2"), ToolbarStyles.commandButtonStyle))
            // {
            //     SceneHelper.StartScene("Assets/ToolbarExtender/Example/Scenes/Scene2.unity");
            // }
        }
        
        static void SetPlayModeStartScene(string scenePath)
        {
            SceneAsset myWantedStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
            if (myWantedStartScene != null)
                EditorSceneManager.playModeStartScene = myWantedStartScene;
            else
                Debug.Log("Could not find Scene " + scenePath);
        }
    }
}