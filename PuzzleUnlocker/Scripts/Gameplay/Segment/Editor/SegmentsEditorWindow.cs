using System.Collections.Generic;
using System.Linq;
using PuzzleUnlocker.Gameplay.Content;
using PuzzleUnlocker.Gameplay.Ring;
using PuzzleUnlocker.Gameplay.Segment;
using TheTide.utils;
using TriangleNet;
using TriangleNet.Geometry;
using UnityEditor;
using UnityEngine;

namespace PuzzleUnlocker.Segment.Editor
{
    public class SegmentsEditorWindow : EditorWindow
    {
        private int _segmentsCount = 12;
        private float _bigRadius = 25;
        private float _smallRadius = 20;
        private int _blur = 25;
        private int _space = 0;
        private SegmentView _segmentView;
        private AGameplayContent _content;
        private RingViewBase _ring;
        
        
        [MenuItem("PuzzleUnLocker/Segments Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<SegmentsEditorWindow>();
            window.titleContent = new GUIContent("Segments Editor");
            window.Show();
        }

        private void OnGUI()
        {
            _segmentView = EditorGUILayout.ObjectField(_segmentView, typeof(SegmentView), true) as SegmentView;
            _content = EditorGUILayout.ObjectField(_content, typeof(AGameplayContent), true) as AGameplayContent;
            _segmentsCount = EditorGUILayout.IntField("Segments Count", _segmentsCount);
            _bigRadius = EditorGUILayout.FloatField("Big Radius", _bigRadius);
            _smallRadius = EditorGUILayout.FloatField("Small Radius", _smallRadius);
            _blur = EditorGUILayout.IntSlider("Blur", _blur, 5, 200);
            _space = EditorGUILayout.IntField("Space", _space);
            
            if (GUILayout.Button("Create"))
            {
                var ringGenerator = new RingGenerator(_segmentView, _segmentsCount, _bigRadius, _smallRadius, _blur, _space, _content);
                _ring = ringGenerator.Create();
            }

            if (GUILayout.Button("Update Segment") && _segmentView != null)
            {

                var path = "Assets/PuzzleUnlocker/Prefabs/Resources/" + _segmentView.gameObject.name + ".prefab";
                var ringGenerator = new RingGenerator(_segmentView, _segmentsCount, _bigRadius, _smallRadius, _blur, _space, _content);
                
                var prefab =  (GameObject)PrefabUtility.InstantiatePrefab(_segmentView.gameObject);;
                ringGenerator.GenerateSegment(0, _content, prefab.GetComponent<SegmentView>());
                var serialize = prefab.GetComponent<SerializeMesh>();
                if (serialize == null)
                {
                    serialize = prefab.AddComponent<SerializeMesh>();
                }
                serialize.Serialize();
                PrefabUtility.SaveAsPrefabAsset(prefab, path);
                GameObject.DestroyImmediate(prefab);
                AssetDatabase.SaveAssets();
            }
        }

    }
}