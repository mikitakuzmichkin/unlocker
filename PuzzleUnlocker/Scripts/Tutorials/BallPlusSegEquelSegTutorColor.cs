using PuzzleUnlocker.Scripts.Gameplay.Ball;
using UnityEngine;

namespace PuzzleUnlocker.Tutorials
{
    public class BallPlusSegEquelSegTutorColor : BallPlusSegEquelSegTutor
    {
        [SerializeField] private TutorialBallCrossColors _tutorialBallCross;
        public override void SetContent(object ball, object seg, object sum)
        {
            var ballColor = (Color)ball;
            var segColor = (Color) seg;
            var sumColor = (Color) sum;
            base.SetContent(ball, seg, ballColor + segColor);
            var value = ballColor + segColor - sumColor;
            _tutorialBallCross.UpdateValue(value);
        }
    }
}