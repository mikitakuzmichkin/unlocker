using PuzzleUnlocker.Gameplay.Segment;
using UnityEngine;

namespace PuzzleUnlocker.Gameplay.Ring
{
    public interface IRingController
    {
        void OnBallCollision(Vector2 point);

        void SetChild(Transform child);

        void SetZPosition(float z);

        SegmentView GetSegmentByNearestPos(Vector2 point);

        int GetSegmentIndexByNearestPos(Vector2 point);

        SegmentView GetSegmentByIndex(int index);
    }
}