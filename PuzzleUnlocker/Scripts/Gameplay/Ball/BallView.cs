using System;
using System.Collections;
using PuzzleUnlocker.Gameplay.Content;
using PuzzleUnlocker.Gameplay.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PuzzleUnlocker.Scripts.Gameplay.Ball
{
    public class BallView : MonoBehaviour
    {
        [SerializeField] private float _radius;
        [SerializeField] private float _speed;
        [SerializeField] private AGameplayContent _content;

        private IGameplayModel _model;
        private Coroutine _ballMoving;
        private Vector3 _center;
        private Vector2 _direction;
        private float _maxBallDistance;

        private bool _pause;

        public AGameplayContent Content
        {
            get => _content;
            set => _content = value;
        }

        public float Radius => _radius * transform.lossyScale.x;

        public event Action<Vector2> Collision;

        public void Init(IGameplayModel model)
        {
            UnInitialize();

            _model = model;
            _model.UpdateBallValue += UpdateBall;
        }

        public void UnInitialize()
        {
            if (_model != null)
            {
                _model.UpdateBallValue -= UpdateBall;
                _model = null;
            }
        }

        public void StartBall(float innerRingRadius)
        {
            _maxBallDistance = innerRingRadius - Radius;
            _center = transform.localPosition;
            _direction = Vector2.right;

            _ballMoving = StartCoroutine(BallCoroutine());
        }

        public void ResumeBall()
        {
            _ballMoving = StartCoroutine(BallCoroutine());
        }

        public void Stop()
        {
            if (_ballMoving != null)
            {
                if (this != null)
                {
                    StopCoroutine(_ballMoving);
                }

                _ballMoving = null;
            }
        }
        
        public void Pause()
        {
            _pause = true;
        }
        
        public void UnPause()
        {
            _pause = false;
        }

        public void UpdateBall(object value)
        {
            Content.UpdateValue(value, ETypeFromContent.Ball);
        }

        private IEnumerator BallCoroutine()
        {
            _direction.Normalize();

            var ballTransform = transform;
            var pos = ballTransform.localPosition;
            while (true)
            {
                pos += (Vector3) _direction * (_speed * Time.deltaTime);

                if (Vector2.Distance(_center, pos) >= _maxBallDistance)
                {
                    pos = _center + (Vector3) (_direction * _maxBallDistance);
                    _direction *= -1;
                    ballTransform.localPosition = pos;
                    Collision?.Invoke(ballTransform.position);
                }
                else
                {
                    ballTransform.localPosition = pos;
                }

                yield return null;
                while (_pause)
                {
                    yield return null;
                }
            }
        }

        private void OnDestroy()
        {
            UnInitialize();
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, Radius);
        }
#endif
    }
}