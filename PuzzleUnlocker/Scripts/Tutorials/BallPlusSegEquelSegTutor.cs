using PuzzleUnlocker.Gameplay.Content;
using PuzzleUnlocker.Gameplay.Segment;
using PuzzleUnlocker.Scripts.Gameplay.Ball;
using UnityEngine;

namespace PuzzleUnlocker.Tutorials
{
    public class BallPlusSegEquelSegTutor : MonoBehaviour
    {
        [SerializeField] private SegmentView _firstSegment;
        [SerializeField] private SegmentView _sumSegment;
        [SerializeField] private BallView _ball;
        [SerializeField] private BallView _sumBall;
        [SerializeField] private GameObject _arrowSegment;
        [SerializeField] private GameObject _arrowBall;

        public virtual void SetContent(object ball, object seg, object sum)
        {
            if (_firstSegment != null)
            {
                _firstSegment.Content.UpdateValue(seg, ETypeFromContent.Segment);
            }

            if (_ball != null)
            {
                _ball.Content.UpdateValue(ball, ETypeFromContent.Ball);
            }
            
            if (_sumSegment != null)
            {
                _sumSegment.Content.UpdateValue(sum, ETypeFromContent.Segment);
            }
            
            if (_sumBall != null)
            {
                _sumBall.Content.UpdateValue(sum, ETypeFromContent.Segment);
            }
        }

        public void ShowSegment()
        {
            _sumSegment.gameObject.SetActive(true);
            _sumBall.gameObject.SetActive(false);
            _arrowSegment.SetActive(true);
            _arrowBall.SetActive(false);
        }

        public void ShowBall()
        {
            if (_sumSegment != null)
            {
                _sumSegment.gameObject.SetActive(false);
            }

            if (_sumBall != null)
            {
                _sumBall.gameObject.SetActive(true);
            }

            if (_arrowBall != null)
            {
                _arrowBall.SetActive(true);
            }

            if (_arrowSegment != null)
            {
                _arrowSegment.SetActive(false);
            }
        }
    }
}