using System;
using System.Collections;
using PuzzleUnlocker.Gameplay.Content;
using PuzzleUnlocker.Gameplay.Model;
using PuzzleUnlocker.Gameplay.Segment;
using PuzzleUnlocker.Scripts.Extensions;
using UnityEngine;

namespace PuzzleUnlocker.Gameplay.Ring
{
    public class RingViewBase : MonoBehaviour
    {
        [SerializeField] private SegmentView[] _segments;

        public int SegmentsCount => _segments.Length;
        [SerializeField] public float InnerRadiusForBaseSegment;
        [SerializeField] public float OuterRadius;
        [SerializeField] public float InnerRadius;

        private IEnumerator Start()
        {
            yield return new WaitWhile(() => RingInput.Instance == null);

            RingInput.Instance.Rotate += Rotate;
        }

        public void UpdateSegments<T>(T[] segmentsValue)
        {
            for (int i = 0; i < segmentsValue.Length; i++)
            {
                _segments[i].Content.UpdateValue(segmentsValue[i], ETypeFromContent.Segment);
            }
        }

        public void UpdateLightSelectedSegments(BitArray lightSelected)
        {
            for (int i = 0; i < lightSelected.Length; i++)
            {
                _segments[i].Content.IsSelect(lightSelected[i]);
            }
        }

        public virtual void SetSelectView(int[] selects)
        {
            if (selects == null)
            {
                return;
            }

            foreach (var select in selects)
            {
                _segments[select].GetComponent<MeshRenderer>().material.color = Color.gray;
            }
        }

        public void AddedSegments(SegmentView[] segments)
        {
            _segments = segments;
        }

        public int GetNearestSegmentIndex(Vector2 point)
        {
            return _segments.MinByIndex(s => Vector3.Distance((Vector2) s.transform.position, point));
        }
        
        public SegmentView GetNearestSegment(Vector2 point)
        {
            return _segments.MinBy(s => Vector3.Distance((Vector2) s.transform.position, point));
        }

        private void OnDestroy()
        {
            RingInput.Instance.Rotate -= Rotate;
        }

        private void Rotate(float angle)
        {
            transform.Rotate(0, 0, angle);
        }
        
        public SegmentView GetSegmentByIndex(int index)
        {
            return _segments[index];
        }
    }
}