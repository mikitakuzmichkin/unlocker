using PuzzleUnlocker.Gameplay.Model;
using PuzzleUnlocker.Gameplay.Segment;
using UnityEngine;

namespace PuzzleUnlocker.Gameplay.Ring
{
    public class RingController<T, TB> : IRingController
    {
        private RingViewBase _view;
        private AGameplayModel<T, TB> _model;

        public RingController(RingViewBase ringView, AGameplayModel<T, TB> model)
        {
            _view = ringView;
            _model = model;
            _view.SetSelectView(_model.SelectIndexes);
        }

        public void Subscribe()
        {
            _model.UpdatedSegments += _view.UpdateSegments;
            _model.UpdatedSelectedLightSegments += _view.UpdateLightSelectedSegments;
        }

        public void UnSubscribe()
        {
            _model.UpdatedSegments -= _view.UpdateSegments;
            _model.UpdatedSelectedLightSegments -= _view.UpdateLightSelectedSegments;
        }

        public void OnBallCollision(Vector2 point)
        {
            var index = _view.GetNearestSegmentIndex(point);
            _model.BallConnectWithSegment(index);
        }
        
        public SegmentView GetSegmentByNearestPos(Vector2 point)
        {
            return _view.GetNearestSegment(point);
        }

        public int GetSegmentIndexByNearestPos(Vector2 point)
        {
            return _view.GetNearestSegmentIndex(point);
        }

        public SegmentView GetSegmentByIndex(int index)
        {
            return _view.GetSegmentByIndex(index);
        }

        public void SetChild(Transform child)
        {
            child.SetParent(_view.transform.parent);
            child.localPosition = Vector3.zero;
            child.localScale = Vector3.one;
        }

        public void SetZPosition(float z)
        {
            var parent = _view.transform.parent;
            var pos = parent.position;
            pos.z = z;
            parent.position = pos;
        }
    }
}