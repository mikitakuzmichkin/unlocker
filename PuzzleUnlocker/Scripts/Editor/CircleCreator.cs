using System.Collections.Generic;
using System.Linq;
using TriangleNet;
using TriangleNet.Geometry;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PuzzleUnlocker.Editor
{
    public class CircleCreator : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("GameObject/2D Object/MeshCircle")]
        public static void Create()
        {
            var blur = 360;
            var radius = 1;
            var points = GetCirclePoints(0, 360, blur, radius);
            var poly = new Polygon();
            poly.Add(points);
            var triangleNetMesh = (TriangleNetMesh) poly.Triangulate();
            
            var go = new GameObject("circle");
            var mf = go.AddComponent<MeshFilter>();
            var mesh = triangleNetMesh.GenerateUnityMesh();

            mf.mesh = mesh;

            var mr = go.AddComponent<MeshRenderer>();
        }
        
        private static List<Vector2> GetCirclePoints(float from, float to, int stepsCount, float radius)
        {
            var points = new List<Vector2>();
            var step = (to - from) / stepsCount;
            var point = new Vector2();
            for (var i = from; Mathf.Abs(i - to) > Mathf.Abs(step); i += step)
            {
                point.x = Mathf.Cos(i * Mathf.PI / 180f) * radius;
                point.y = Mathf.Sin(i * Mathf.PI / 180f) * radius;
                points.Add(point);
            }

            point = new Vector2();
            point.x = Mathf.Cos(to * Mathf.PI / 180f) * radius;
            point.y = Mathf.Sin(to * Mathf.PI / 180f) * radius;
            points.Add(point);

            return points;
        }
#endif
    }
}