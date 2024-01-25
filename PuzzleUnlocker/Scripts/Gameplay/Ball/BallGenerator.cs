using System;
using PuzzleUnlocker.Gameplay.Content;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PuzzleUnlocker.Scripts.Gameplay.Ball
{
    public class BallGenerator
    {
        private const float SQRT2 = 1.4142135623f;

        public BallView Create(Vector3 position, AGameplayContent content = null, BallView ballPref = null, Transform parent = null)
        {
            if (ballPref == null)
            {
                ballPref = Resources.Load<GameObject>("Ball/BallDefault").GetComponent<BallView>();
            }
            var ball = Object.Instantiate(ballPref, position, Quaternion.identity, parent);

            if (ball.Content != null)
            {
                content = ball.Content;
               
            }
            else
            {
                if (content == null)
                {
                    throw new NullReferenceException("Content is not found");
                }
                
                content = Object.Instantiate(content.gameObject, ball.transform).GetComponent<AGameplayContent>();
                ball.Content = content;
                var side = (ball.Radius * SQRT2) / ball.transform.lossyScale.x;
                content.RectTransform.sizeDelta = new Vector2(side, side);
            }

            return ball;
        }
    }
}