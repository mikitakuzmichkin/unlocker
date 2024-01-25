using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace PuzzleUnlocker.Editor
{
    public class EditorMenuTools
    {
        private const string SCREENSHOTS_DIR_NAME = "Screenshots";

        [MenuItem("Tools/Take Screenshot")]
        private static void Screenshot()
        {
            if (!Directory.Exists(SCREENSHOTS_DIR_NAME))
            {
                Directory.CreateDirectory(SCREENSHOTS_DIR_NAME);
            }

            var filename = $"{SCREENSHOTS_DIR_NAME}/{Guid.NewGuid()}.png";
            ScreenCapture.CaptureScreenshot(filename, 1);
            Debug.Log($"Screenshot \"{filename}\" created!");
        }

        [MenuItem("Tools/Open PlayerPrefs")]
        public static void OpenPlayerPrefs()
        {
#if UNITY_EDITOR_WIN
            var registryLocation =
                $@"HKEY_CURRENT_USER\Software\Unity\UnityEditor\{Application.companyName}\{Application.productName}";
            const string registryLastKey =
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Applets\Regedit";
            try
            {
                // Set LastKey value that regedit will go directly to
                Microsoft.Win32.Registry.SetValue(registryLastKey, "LastKey", registryLocation);
                Process.Start("regedit.exe");
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
#endif
        }

        [MenuItem("Tools/Clear All Data")]
        public static void ClearAllData()
        {
            ClearDirectory(Application.persistentDataPath);
            ClearDirectory(Application.temporaryCachePath);

            Caching.ClearCache();

            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        private static void ClearDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            var rootDir = new DirectoryInfo(path);

            foreach (var file in rootDir.GetFiles())
            {
                file.Delete();
            }

            foreach (var dir in rootDir.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}