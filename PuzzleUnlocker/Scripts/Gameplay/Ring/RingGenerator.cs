using System;
using System.Collections.Generic;
using System.Linq;
using PuzzleUnlocker.Gameplay.Content;
using PuzzleUnlocker.Gameplay.Segment;
using TheTide.utils;
using TriangleNet;
using TriangleNet.Geometry;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PuzzleUnlocker.Gameplay.Ring
{
    public class RingGenerator
    {
        private readonly int _segmentsCount = 12;
        private readonly float _bigRadius = 25;
        private readonly float _smallRadius = 20;
        private readonly int _blur = 25;
        private readonly int _space;
        private readonly SegmentView _segmentView;
        private readonly AGameplayContent _gameplayContent;

        public RingGenerator(SegmentView segmentView, int segmentsCount, float bigRadius, float smallRadius, int blur,
            int space, AGameplayContent content = null)
        {
            _segmentsCount = segmentsCount;
            _bigRadius = bigRadius;
            _smallRadius = smallRadius;
            _blur = blur;
            _space = space;
            _segmentView = segmentView;
            _gameplayContent = content;
        }

        public RingViewBase Create()
        {
            var ringGO = new GameObject("Ring");
            var ringView = ringGO.AddComponent<RingViewBase>();
            ringView.InnerRadiusForBaseSegment = _smallRadius;
            ringView.OuterRadius = _bigRadius;
            var segmentViews = new List<SegmentView>();

            float minRadius = _smallRadius;

            var angleStep = 360f / _segmentsCount;
            for (float angle = 0; angle < 360; angle += angleStep)
            {
                var segmentView = GenerateSegment(angle, _gameplayContent);
                
                segmentView.InnerRadius = _smallRadius;
                segmentView.OuterRadius = _bigRadius;

                var additiveSegmentsViews = segmentView.GetComponentsInChildren<SegmentView>(false)
                    .Where(s => s.gameObject != segmentView.gameObject);

                foreach (var additiveSegment in additiveSegmentsViews)
                {
                    GenerateAdditiveSegment(angle, additiveSegment);
                    if (minRadius > additiveSegment.InnerRadius)
                    {
                        minRadius = additiveSegment.InnerRadius;
                    }
                }
                
                ringView.InnerRadius = minRadius;

                segmentView.transform.SetParent(ringGO.transform);
                segmentViews.Add(segmentView);
            }

            ringView.AddedSegments(segmentViews.ToArray());
            return ringView;
        }

        public SegmentView GenerateSegment(float angle, AGameplayContent content = null, SegmentView segmentView = null)
        {
            float bigRadius = _bigRadius;
            float smallRadius = _smallRadius;
            if (segmentView == null)
            {
                segmentView = CreateSegment();
                // bigRadius = segmentView.OuterRadius;
                // smallRadius = segmentView.InnerRadius;
            }

            var middleRadius = (bigRadius - smallRadius) / 2f + smallRadius;
            var differenceBetweenRadius = bigRadius - smallRadius;

            var position = new Vector2
            {
                x = Mathf.Cos(angle * Mathf.Deg2Rad) * middleRadius,
                y = Mathf.Sin(angle * Mathf.Deg2Rad) * middleRadius
            };

            var poly = new Polygon();
            var points = GetPoints(bigRadius, smallRadius);
            for (var i = 0; i < points.Count; i++)
            {
                var vec = points[i];
                vec.y -= middleRadius;
                points[i] = vec;
            }

            poly.Add(points);

            var triangleNetMesh = (TriangleNetMesh) poly.Triangulate();

            var segmentGO = segmentView.gameObject;
            //segmentGO.transform.position = Vector3.zero;
            segmentGO.transform.position = position;

            var mf = segmentGO.GetComponent<MeshFilter>();
            var mesh = triangleNetMesh.GenerateUnityMesh();
            mesh.uv = points.Select(p =>
            {
                var point = p;
                var angle = (180f - (360f / _segmentsCount)) / 360f * Mathf.PI;
                point.x /= 2 * bigRadius * Mathf.Cos(angle);
                point.y /= differenceBetweenRadius;
                point += new Vector2(0.5f, 0.5f);
                return point;
            }).ToArray();

            mf.mesh = mesh;

            segmentGO.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            if (segmentView.Content != null)
            {
                content = segmentView.Content;
            }
            else
            {
                if (content == null)
                {
                    throw new NullReferenceException("Content is not found");
                }

                content = Object.Instantiate(content.gameObject).GetComponent<AGameplayContent>();
                content.transform.SetParent(segmentView.transform, false);
                segmentView.Content = content;
            }

            // var difference = differenceBetweenRadius / content.RectTransform.sizeDelta.x;
            // content.RectTransform.localScale = new Vector3(difference,difference);
            content.RectTransform.sizeDelta = new Vector2(differenceBetweenRadius, differenceBetweenRadius);

            return segmentView;
        }
        
        public SegmentView GenerateAdditiveSegment(float angle, SegmentView segmentView)
        {
            var middleRadius = (segmentView.OuterRadius - segmentView.InnerRadius) / 2f + segmentView.InnerRadius;
            var differenceBetweenRadius = segmentView.OuterRadius - segmentView.InnerRadius;

            var poly = new Polygon();
            var points = GetPoints(segmentView.OuterRadius, segmentView.InnerRadius);
            for (var i = 0; i < points.Count; i++)
            {
                var vec = points[i];
                vec.y -= middleRadius;
                points[i] = vec;
            }

            poly.Add(points);

            var triangleNetMesh = (TriangleNetMesh) poly.Triangulate();

            var segmentGO = segmentView.gameObject;
            //segmentGO.transform.localPosition = Vector3.zero;

            var mf = segmentGO.GetComponent<MeshFilter>();
            var mesh = triangleNetMesh.GenerateUnityMesh();
            mesh.uv = points.Select(p =>
            {
                var point = p;
                var angle = (180f - (360f / _segmentsCount)) / 360f * Mathf.PI;
                point.x /= 2 * segmentView.OuterRadius * Mathf.Cos(angle);
                point.y /= differenceBetweenRadius;
                point += new Vector2(0.5f, 0.5f);
                return point;
            }).ToArray();

            mf.mesh = mesh;
            
            segmentGO.GetComponent<SerializeMesh>().Serialize();

            segmentGO.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            return segmentView;
        }

        private SegmentView CreateSegment()
        {
            SegmentView segmentView;
            if (_segmentView == null)
            {
                var segmentLoad = Resources.Load<GameObject>("Segment/SegmentDefault").GetComponent<SegmentView>();
                segmentView = Object.Instantiate(segmentLoad);
            }
            else
            {
                segmentView = Object.Instantiate(_segmentView);
            }

            return segmentView;
        }

        private List<Vector2> GetPoints(float bigRadius, float smallRadius)
        {
            var angle = 360f / _segmentsCount;
            var halfAngle = angle / 2f - _space / 2f;

            var points = new List<Vector2>();
            GetCirclePoints(90 + halfAngle, 90 - halfAngle, _blur, bigRadius, points);
            GetCirclePoints(90 - halfAngle, 90 + halfAngle, _blur, smallRadius, points);
            return points;
        }

        private static void GetCirclePoints(float from, float to, int stepsCount, float radius,
            ICollection<Vector2> output)
        {
            var step = (to - from) / stepsCount;
            var point = new Vector2();
            for (var i = from; Mathf.Abs(i - to) > Mathf.Abs(step); i += step)
            {
                point.x = Mathf.Cos(i * Mathf.PI / 180f) * radius;
                point.y = Mathf.Sin(i * Mathf.PI / 180f) * radius;
                output.Add(point);
            }

            point = new Vector2
            {
                x = Mathf.Cos(to * Mathf.PI / 180f) * radius,
                y = Mathf.Sin(to * Mathf.PI / 180f) * radius
            };
            output.Add(point);
        }
    }
}