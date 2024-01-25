using System;
using System.Collections.Generic;
using System.Linq;
using PuzzleUnlocker.Scripts.Extensions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace PuzzleUnlocker.Gameplay.Content
{
    public class GameplayFigureContent : AGameplayContent
    {
        [SerializeField] private UILineRenderer _origin;
        [SerializeField] private UILineRenderer _select;
        [SerializeField] private Image _point;

        [Header("Colors")] 
        [SerializeField] private Color _colorInSegment = Color.black;
        [SerializeField] private Color _colorInTarget = Color.black;
        [SerializeField] private Color _colorInBall = Color.white;

        private void Awake()
        {
            if (_select.material != null)
            {
                _select.material = Instantiate(_select.material);
            }
        }
        
        public override void UpdateValue(object value, ETypeFromContent fromContent)
        {
            if (value is null)
            {
                _point.enabled = _origin.enabled = _select.enabled = false;
                return;
            }

            _point.enabled = _origin.enabled = _select.enabled = true;

            _origin.color = fromContent switch
            {
                ETypeFromContent.Ball => _colorInBall,
                ETypeFromContent.Target => _colorInTarget,
                ETypeFromContent.Segment => _colorInSegment,
                _ => throw new ArgumentOutOfRangeException(nameof(fromContent), fromContent, null)
            };

            if (value is Figure figure)
            {
                Line(figure);
                return;
            }
            
            if (value is int i)
            {
                Point(i);
                return;
            }
            
            throw new ArgumentException();
        }

        protected override Material Material => _select.material;
        
        public void LineLerp(Figure figure, bool positiveDirection, float t)
        {
            _select.gameObject.SetActive(true);

            var firstLines = figure.GetLines();
            var points = firstLines.Select(l => _origin.Points[l]).ToList();
            var lastInd = firstLines.Last();
            int nextInd = lastInd;

            if (positiveDirection)
            {
                nextInd++;
            }
            else
            {
                nextInd--;
            }
            
            nextInd = nextInd.LoopCrop(0, figure.Max);
            var point = Vector2.Lerp(_origin.Points[lastInd], _origin.Points[nextInd], t);
            
            if (!(positiveDirection ^ figure.PositiveDirection))
            {
                points[points.Count - 1] = point;
            }
            else
            {
                points.Add(point);
            }
            
            _select.Points = points.ToArray();
            _point.rectTransform.anchoredPosition = point;
        }

        private void Line(Figure figure)
        {
            _select.gameObject.SetActive(true);
            var lines = figure.GetLines();
            if (lines != null)
            {
                var points = lines.Select(l => _origin.Points[l]);
                _select.Points = points.ToArray();
            }
            else
            {
                _select.gameObject.SetActive(false);
            }
            Point(figure.GetCurrentIndex());
        }

        private void Point(int index)
        {
            _point.rectTransform.anchoredPosition = _origin.Points[index];
        }
    }
}