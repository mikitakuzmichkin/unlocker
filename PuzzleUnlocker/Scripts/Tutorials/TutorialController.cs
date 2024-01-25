using System;
using System.Collections.Generic;
using System.Linq;
using Dainty.Ads;
using Dainty.DI;
using DG.Tweening;
using PuzzleUnlocker.Data;
using PuzzleUnlocker.Gameplay;
using PuzzleUnlocker.Gameplay.Level;
using PuzzleUnlocker.Gameplay.Model;
using PuzzleUnlocker.Gameplay.Ring;
using PuzzleUnlocker.Scripts.Extensions;
using PuzzleUnlocker.Scripts.Gameplay.Ball;
using PuzzleUnlocker.Services.Ads;
using PuzzleUnlocker.UI.Windows;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PuzzleUnlocker.Tutorials
{
    public class TutorialController
    {
        private List<TutorialStep> _activeSteps;
        private EGameType _activeType;
        private TutorialView _view;

        public IRingController RingController { get; set; }
        public BallView BallView { get; set; }
        public IGameplayModel GameplayModel { get; set; }
        public GameWindowController GameWindowController { get; set; }

        public event Action NeedPause;

        public ALevelData LevelData { get; private set; }

        public void Init(TutorialView tutorialView)
        {
            _view = tutorialView;
        }

        public int GetLevelsBefore(int curLevel, bool isTutorial)
        {
            return ProjectContext.GetInstance<ITutorialDataProvider>().GetLevelsBefore(curLevel, isTutorial);
        }

        public ALevelData TryGetTutorialLevels(int currentLevel)
        {
            var checker = ProjectContext.GetInstance<ITutorialDataProvider>()
                .TryGetTutorialsData(currentLevel, out var tutorialSteps, out var type);

            if (checker == false)
            {
                _activeSteps = null;
                return null;
            }

            _activeType = type;
            //ActivateSteps(type, tutorialSteps);

            _activeSteps = tutorialSteps;

            if (_activeSteps == null || _activeSteps.Count < 1)
            {
                return null;
            }

            return tutorialSteps[0]?.LevelData;
        }

        public void CheckTutorByStart()
        {
            if (_activeSteps == null)
            {
                return;
            }

            if (_activeType == EGameType.Number)
            {
                if (_activeSteps.FirstOrDefault(s => s.Id == 0) != null)
                {
                    GameplayModel.ForceSetBallValue(1);
                    NumberStep0();
                }
            }
            
            if (_activeType == EGameType.Cube)
            {
                if (_activeSteps?.FirstOrDefault(s => s.Id == 0) != null)
                {
                    GameplayModel.ForceSetBallValue(5);
                    CubeStep0();
                }
            }
            
            if (_activeType == EGameType.Color)
            {
                if (_activeSteps?.FirstOrDefault(s => s.Id == 0) != null)
                {
                    GameplayModel.ForceSetBallValue(new Color(1,0,1));
                    ColorStep0();
                }
            }
            
            if (_activeType == EGameType.Figure)
            {
                if (_activeSteps?.FirstOrDefault(s => s.Id == 0) != null)
                {
                    GameplayModel.ForceSetBallValue(0);
                    FigureStep0();
                }
            }
        }

        public void CheckTutorByBallCollision(int count)
        {
            switch (count)
            {
                case 1:
                    if (_activeType == EGameType.Number)
                    {
                        if (_activeSteps?.FirstOrDefault(s => s.Id == 1) != null)
                        {
                            NumberStep1();
                        }
                    }
                    
                    if (_activeType == EGameType.Cube)
                    {
                        if (_activeSteps?.FirstOrDefault(s => s.Id == 1) != null)
                        {
                            CubeStep1();
                        }
                    }
                    
                    if (_activeType == EGameType.Color)
                    {
                        if (_activeSteps?.FirstOrDefault(s => s.Id == 1) != null)
                        {
                            ColorStep1();
                        }
                    }
                    
                    if (_activeType == EGameType.Figure)
                    {
                        if (_activeSteps?.FirstOrDefault(s => s.Id == 1) != null)
                        {
                            FigureStep1();
                        }
                    }

                    break;
                case 2:
                    if (_activeType == EGameType.Number)
                    {
                        if (_activeSteps?.FirstOrDefault(s => s.Id == 3) != null)
                        {
                            NumberStep3();
                        }
                    }

                    break;
            }
        }

        public void SaveTutorial()
        {
            var id = _activeSteps.Last().Id;
            ProjectContext.GetInstance<IPuDataProvider>().SetTutorialLevelId(id, _activeType);
        }

        private void ActivateSteps(EGameType type, List<TutorialStep> tutorialSteps)
        {
            var lastStep = ProjectContext.GetInstance<IPuDataProvider>().GetTutorialLevelId(type);
            _activeSteps = tutorialSteps.Where(s => s.Id > lastStep).ToList();
            //_activeSteps = tutorialSteps;
        }
        
        #region Numbers
        
        private void NumberStep0()
        {
            var circleArrowWithHand = _view.NewCircleArrowWithHand;
            RingController.SetChild(circleArrowWithHand.transform);
            _view.SetRulesText("Spin the wheel to play");
            //ProjectContext.GetInstance<IPuAdsController>().HideBanner();
            _view.BlackPanel.SetActive(true);
            _view.TargetCount.SetActive(false);
            _view.TargetValue.SetActive(false);
            RingController.SetZPosition(-8);

            void Cancel()
            {
                _view.TargetCount.SetActive(true);
                _view.TargetValue.SetActive(true);

                circleArrowWithHand.SetActive(false);
                _view.SetRulesText("");
                _view.BlackPanel.SetActive(false);
                RingController.SetZPosition(0);
                _view.TutorialClick -= Cancel;

                NumberStep2();
            }

            _view.TutorialClick += Cancel;
        }

        private void NumberStep1()
        {
            _view.NumSum.gameObject.SetActive(true);
            _view.SetRulesText("The values add up");
            _view.BlackPanel.SetActive(true);
            _view.TargetCount.SetActive(false);
            _view.TargetValue.SetActive(false);
            BallView.Pause();
            RingInput.Instance.EndDrag = true;
            var segment = RingController.GetSegmentByNearestPos(BallView.transform.position);
            var segmentIndex = RingController.GetSegmentIndexByNearestPos(BallView.transform.position);
            var segmentValue = (int) GameplayModel.GetSegmentValue(segmentIndex);
            var ballValue = (int) GameplayModel.GetBallValue();
            _view.NumSum.SetContent(ballValue, segmentValue, ballValue + segmentValue);
            _view.NumSum.ShowSegment();

            var transformSeg = segment.transform;

            var posSeg = transformSeg.position;
            var prevZSeg = posSeg.z;
            posSeg.z = -8;
            transformSeg.position = posSeg;

            var transformBall = BallView.transform;

            var posBall = transformBall.position;
            var prevZBall = posBall.z;
            posBall.z = -8;
            transformBall.position = posBall;

            void Next()
            {
                var posSeg = transformSeg.position;
                posSeg.z = prevZSeg;
                transformSeg.position = posSeg;

                _view.TutorialClick -= Next;

                _view.NumSum.ShowBall();

                void Cancel()
                {
                    _view.NumSum.gameObject.SetActive(false);
                    _view.TargetCount.SetActive(true);
                    _view.TargetValue.SetActive(true);
                    _view.BlackPanel.SetActive(false);
                    _view.SetRulesText("");
                    posBall.z = prevZBall;
                    transformBall.position = posBall;
                    BallView.UnPause();
                    _view.TutorialClick -= Cancel;
                }

                _view.TutorialClick += Cancel;
            }

            _view.TutorialClick += Next;
        }

        private void NumberStep2()
        {
            _view.TargetArrow.gameObject.SetActive(true);
            _view.SetRulesText("You need to get a positive number inside the highlighted segment");
            _view.BlackPanel.SetActive(true);
            _view.TargetCount.SetActive(false);

            BallView.Pause();

            var segment = RingController.GetSegmentByIndex(1);
            var transformSeg = segment.transform;

            var posSeg = transformSeg.position;
            var prevZSeg = posSeg.z;
            posSeg.z = -8;
            transformSeg.position = posSeg;

            var targetParent = _view.TargetPanel.transform.parent;
            _view.TargetPanel.transform.SetParent(_view.transform);

            void Cancel()
            {
                BallView.UnPause();
                _view.TargetPanel.transform.SetParent(targetParent);
                _view.TargetCount.SetActive(true);
                var posSeg = transformSeg.position;
                posSeg.z = prevZSeg;
                transformSeg.position = posSeg;

                _view.TargetArrow.gameObject.SetActive(false);
                _view.SetRulesText("");
                _view.BlackPanel.SetActive(false);
                _view.TutorialClick -= Cancel;
            }

            _view.TutorialClick += Cancel;
        }

        private void NumberStep3()
        {
            var loseRules = _view.NewLoseRules;
            _view.SetRulesText("If the segment is more than 9 or less than -9, you will lose");
            _view.BlackPanel.SetActive(true);
            _view.TargetCount.SetActive(false);
            _view.TargetValue.SetActive(false);
            BallView.Pause();
            RingInput.Instance.EndDrag = true;

            void Cancel()
            {
                BallView.UnPause();
                _view.TargetCount.SetActive(true);
                _view.TargetValue.SetActive(true);
                loseRules.SetActive(false);
                _view.SetRulesText("");
                _view.BlackPanel.SetActive(false);
                _view.TutorialClick -= Cancel;
            }

            _view.TutorialClick += Cancel;
        }
        
        #endregion

        #region Cube

        private void CubeStep0()
        {
            var cubeReverse = _view.CubeReverse;
            cubeReverse.SetActive(true);
            _view.SetRulesText("The cube values change cyclically");
            _view.BlackPanel.SetActive(true);
            _view.TargetCount.SetActive(false);
            _view.TargetValue.SetActive(false);
            BallView.Pause();
            RingInput.Instance.EndDrag = true;

            void Cancel()
            {
                BallView.UnPause();
                _view.TargetCount.SetActive(true);
                _view.TargetValue.SetActive(true);
                cubeReverse.SetActive(false);
                _view.SetRulesText("");
                _view.BlackPanel.SetActive(false);
                _view.TutorialClick -= Cancel;
            }

            _view.TutorialClick += Cancel;
        }
        
        private void CubeStep1()
        {
            _view.CubeSum.gameObject.SetActive(true);
            _view.SetRulesText("The values add up");
            _view.BlackPanel.SetActive(true);
            _view.TargetCount.SetActive(false);
            _view.TargetValue.SetActive(false);
            BallView.Pause();
            RingInput.Instance.EndDrag = true;
            var segment = RingController.GetSegmentByNearestPos(BallView.transform.position);
            var segmentIndex = RingController.GetSegmentIndexByNearestPos(BallView.transform.position);
            var segmentValue = (int) GameplayModel.GetSegmentValue(segmentIndex);
            var ballValue = (int) GameplayModel.GetBallValue();
            var sum = (ballValue + segmentValue) % 6;
            _view.CubeSum.SetContent(ballValue, segmentValue, sum);
            _view.CubeSum.ShowSegment();

            var transformSeg = segment.transform;

            var posSeg = transformSeg.position;
            var prevZSeg = posSeg.z;
            posSeg.z = -8;
            transformSeg.position = posSeg;

            void Cancel()
            {
                var posSeg = transformSeg.position;
                posSeg.z = prevZSeg;
                transformSeg.position = posSeg;
                
                _view.CubeSum.gameObject.SetActive(false);
                _view.TargetCount.SetActive(true);
                _view.TargetValue.SetActive(true);
                _view.BlackPanel.SetActive(false);
                _view.SetRulesText("");
                BallView.UnPause();
                
                _view.TutorialClick -= Cancel;
                
                CubeStep2();
            }

            _view.TutorialClick += Cancel;
        }
        
        private void CubeStep2()
        {
            var cubeRules = _view.CubeFail;
            cubeRules.SetActive(true);
            _view.SetRulesText("Six plus six is a loss");
            _view.BlackPanel.SetActive(true);
            _view.TargetCount.SetActive(false);
            _view.TargetValue.SetActive(false);
            BallView.Pause();
            RingInput.Instance.EndDrag = true;

            void Cancel()
            {
                BallView.UnPause();
                _view.TargetCount.SetActive(true);
                _view.TargetValue.SetActive(true);
                cubeRules.SetActive(false);
                _view.SetRulesText("");
                _view.BlackPanel.SetActive(false);
                _view.TutorialClick -= Cancel;
            }

            _view.TutorialClick += Cancel;
        }
        
        #endregion

        #region Color

        private void ColorStep0()
        {
            var colorStart = _view.ColorStart;
            colorStart.SetActive(true);
            _view.SetRulesText("Different colors add up, same colors are excluded");
            _view.BlackPanel.SetActive(true);
            _view.TargetCount.SetActive(false);
            _view.TargetValue.SetActive(false);
            BallView.Pause();
            RingInput.Instance.EndDrag = true;

            void Cancel()
            {
                BallView.UnPause();
                _view.TargetCount.SetActive(true);
                _view.TargetValue.SetActive(true);
                colorStart.SetActive(false);
                _view.SetRulesText("");
                _view.BlackPanel.SetActive(false);
                _view.TutorialClick -= Cancel;
            }

            _view.TutorialClick += Cancel;
        }
        
        private void ColorStep1()
        {
            _view.ColorSum.gameObject.SetActive(true);
            _view.SetRulesText("In a collision, colors are mutually exclusive");
            _view.BlackPanel.SetActive(true);
            _view.TargetCount.SetActive(false);
            _view.TargetValue.SetActive(false);
            BallView.Pause();
            RingInput.Instance.EndDrag = true;
            var segment = RingController.GetSegmentByNearestPos(BallView.transform.position);
            var segmentIndex = RingController.GetSegmentIndexByNearestPos(BallView.transform.position);
            var segmentValue = (Color) GameplayModel.GetSegmentValue(segmentIndex);
            var ballValue = (Color) GameplayModel.GetBallValue();
            var sum = segmentValue.XorColor(ballValue);
            _view.ColorSum.SetContent(ballValue, segmentValue, sum);
            _view.ColorSum.ShowSegment();
            _view.ColorSum.ShowBall();

            var transformSeg = segment.transform;

            var posSeg = transformSeg.position;
            var prevZSeg = posSeg.z;
            posSeg.z = -8;
            transformSeg.position = posSeg;

            void Cancel()
            {
                var position = transformSeg.position;
                position.z = prevZSeg;
                transformSeg.position = position;
                
                _view.ColorSum.gameObject.SetActive(false);
                _view.TargetCount.SetActive(true);
                _view.TargetValue.SetActive(true);
                _view.BlackPanel.SetActive(false);
                _view.SetRulesText("");
                BallView.UnPause();
                
                _view.TutorialClick -= Cancel;
                
                Debug.Log("color step0");

                ColorStep2();
            }

            _view.TutorialClick += Cancel;
        }
        
        private void ColorStep2()
        {
            var colorFail = _view.ColorFail;
            colorFail.SetActive(true);
            _view.SetRulesText("Same values is a loss");
            _view.BlackPanel.SetActive(true);
            _view.TargetCount.SetActive(false);
            _view.TargetValue.SetActive(false);
            BallView.Pause();
            RingInput.Instance.EndDrag = true;

            void Cancel()
            {
                BallView.UnPause();
                _view.TargetCount.SetActive(true);
                _view.TargetValue.SetActive(true);
                colorFail.SetActive(false);
                _view.SetRulesText("");
                _view.BlackPanel.SetActive(false);
                _view.TutorialClick -= Cancel;
            }

            _view.TutorialClick += Cancel;
        }
        #endregion
        
        private void FigureStep0()
        {
            var figureStart = _view.FigureStart;
            figureStart.SetActive(true);
            _view.SetRulesText("Lines are drawn or erased in the direction indicated on the ball");
            _view.BlackPanel.SetActive(true);
            _view.TargetCount.SetActive(false);
            _view.TargetValue.SetActive(false);
            BallView.Pause();
            RingInput.Instance.EndDrag = true;

            void Cancel()
            {
                BallView.UnPause();
                _view.TargetCount.SetActive(true);
                _view.TargetValue.SetActive(true);
                figureStart.SetActive(false);
                _view.SetRulesText("");
                _view.BlackPanel.SetActive(false);
                _view.TutorialClick -= Cancel;
            }

            _view.TutorialClick += Cancel;
        }
        
        private void FigureStep1()
        {
            _view.FigureSum.gameObject.SetActive(true);
            _view.SetRulesText("The ball gets the previous segment value");
            _view.BlackPanel.SetActive(true);
            _view.TargetCount.SetActive(false);
            _view.TargetValue.SetActive(false);
            BallView.Pause();
            RingInput.Instance.EndDrag = true;
            var segment = RingController.GetSegmentByNearestPos(BallView.transform.position);
            var segmentIndex = RingController.GetSegmentIndexByNearestPos(BallView.transform.position);
            var segmentValue = (Figure) GameplayModel.GetSegmentValue(segmentIndex);
            var ballValue = (int) GameplayModel.GetBallValue();
            _view.FigureSum.SetContent(ballValue, segmentValue, segmentValue.GetCurrentIndex());
            _view.FigureSum.ShowBall();

            var transform = BallView.transform;

            var pos = transform.position;
            var prevZ = pos.z;
            pos.z = -8;
            transform.position = pos;

            void Cancel()
            {
                var position = transform.position;
                position.z = prevZ;
                transform.position = position;
                
                _view.FigureSum.gameObject.SetActive(false);
                _view.TargetCount.SetActive(true);
                _view.TargetValue.SetActive(true);
                _view.BlackPanel.SetActive(false);
                _view.SetRulesText("");
                BallView.UnPause();
                
                _view.TutorialClick -= Cancel;

                FigureStep2();
            }

            _view.TutorialClick += Cancel;
        }
        
        private void FigureStep2()
        {
            var figureFail = _view.FigureFail;
            figureFail.SetActive(true);
            _view.SetRulesText("Same values is a loss");
            _view.BlackPanel.SetActive(true);
            _view.TargetCount.SetActive(false);
            _view.TargetValue.SetActive(false);
            BallView.Pause();
            RingInput.Instance.EndDrag = true;

            void Cancel()
            {
                BallView.UnPause();
                _view.TargetCount.SetActive(true);
                _view.TargetValue.SetActive(true);
                figureFail.SetActive(false);
                _view.SetRulesText("");
                _view.BlackPanel.SetActive(false);
                _view.TutorialClick -= Cancel;
            }

            _view.TutorialClick += Cancel;
        }
    }
}