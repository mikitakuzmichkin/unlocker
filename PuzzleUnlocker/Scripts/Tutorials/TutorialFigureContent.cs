using System;
using DG.Tweening;
using PuzzleUnlocker.Gameplay;
using PuzzleUnlocker.Gameplay.Content;
using UnityEngine;

namespace PuzzleUnlocker.Tutorials
{
    public class TutorialFigureContent : MonoBehaviour
    {
        [SerializeField] private GameplayFigureContent _gameplayFigureContent;
        
        [SerializeField] private int _maxInd = 3;
        [SerializeField] private int _startInd;
        [SerializeField] private int _linesCount;
        [SerializeField] private bool _positiveDirection = true;
        

        [Header("Anim")] 
        [SerializeField] private float _animDuration = 1f;
        [SerializeField] private bool _animOn;

        private Tweener _twin;
        
        private void OnEnable()
        {
            if (_animOn)
            {
                _twin = DOVirtual.Float(0, 1, _animDuration, FigureAnim).SetLoops(-1);
            }
        }

        private void OnDisable()
        {
            _twin?.Kill();
        }

        private void FigureAnim(float t)
        {
            _gameplayFigureContent.LineLerp(new Figure(_startInd, _linesCount, _maxInd), 
                _positiveDirection, t);
        }

        private void OnValidate()
        {
            if (_gameplayFigureContent == null)
            {
                return;
            }

            var figure = new Figure(_startInd, _linesCount, _maxInd);
            _gameplayFigureContent.UpdateValue(figure, ETypeFromContent.Segment);
        }
    }
}