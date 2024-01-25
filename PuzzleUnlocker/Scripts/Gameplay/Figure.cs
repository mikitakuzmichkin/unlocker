using System.Collections.Generic;
using System.Linq;
using PuzzleUnlocker.Scripts.Extensions;
using PuzzleUnlocker.Scripts.Gameplay.Ball;
using UnityEngine;

namespace PuzzleUnlocker.Gameplay
{
    public class Figure
    {
        private int _pointInd;
        private List<Line> _lines;
        private int _maxInd;

        private bool _positiveDirection;

        public Figure(int startInd, int linesCount, int maxInd)
        {
            _lines = new List<Line>();
            _pointInd = (startInd + linesCount).LoopCrop(0, maxInd);
            _maxInd = maxInd;

            _positiveDirection = linesCount >= 0;
            var step = _positiveDirection ? -1 : 1;
            linesCount = Mathf.Abs(linesCount);
            for (int i = 0; i < linesCount; i ++ )
            {
                Move(step);
            }
        }

        public int Max => _maxInd;

        public bool PositiveDirection => _positiveDirection;

        public void ToRight()
        {
            Move(1);
        }

        public void ToLeft()
        {
            Move(-1);
        }

        public void ToIndex(int index)
        {
            Move(index - _pointInd);
        }

        private void Move(int step)
        {
            var nextPoint = _pointInd + step;
            nextPoint = nextPoint.LoopCrop(0, Max);
            
            var line = _lines.FirstOrDefault(l => l.TryGetLine(_pointInd, nextPoint) != null);
            if (line != null)
            {
                _lines.Remove(line);
            }
            else
            {
                _lines.Add(new Line(_pointInd, nextPoint));
            }
            
            _pointInd = nextPoint;
        }

        public bool FullCircle()
        {
            return _lines.Count == Max;
        }

        public int GetCurrentIndex()
        {
            return _pointInd;
        }

        public List<int> GetLines()
        {
            if (_lines.Count < 1)
            {
                return null;
            }
            
            var points = new List<int>();
            points.Add(_lines[0].Point1);
            points.AddRange(_lines.Select( l => l.Point2));
            return points;
        }
        
        public class Line
        {
            public int Point1;
            public int Point2;

            public Line(int point1, int point2)
            {
                Point1 = point1;
                Point2 = point2;
            }

            public Line TryGetLine(int point1, int point2)
            {
                if ((point1 == Point1 && point2 == Point2) ||
                    (point1 == Point2 && point2 == Point1))
                {
                    return this;
                }

                return null;
            }
        }
    }
}