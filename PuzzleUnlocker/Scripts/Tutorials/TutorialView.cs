using System;
using PuzzleUnlocker.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PuzzleUnlocker.Tutorials
{
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _rulesText;
        [SerializeField] private GameObject _blackPanel;
        [SerializeField] private GameObject _targetCount;
        [SerializeField] private GameObject _targetValue;
        [SerializeField] private PointerButton _tutorialButton;
        [SerializeField] private GameObject _targetPanel;
        [SerializeField] private GameObject _TextArea;
        
        [Header("Numbers")]
        [SerializeField] private GameObject _circleArrowWithHand;
        [SerializeField] private BallPlusSegEquelSegTutor _numSum;
        [SerializeField] private GameObject _targetArrow;
        [SerializeField] private GameObject _loseRules;
        
        [Header("Cube")]
        [SerializeField] private GameObject _cubeReverse;
        [SerializeField] private BallPlusSegEquelSegTutor _cubeSum;
        [SerializeField] private GameObject _cubeFail;

        [Header("Colors")] 
        [SerializeField] private GameObject _colorStart;
        [SerializeField] private BallPlusSegEquelSegTutor _colorSum;
        [SerializeField] private GameObject _colorFail;

        [Header("Figure")] 
        [SerializeField] private GameObject _figureStart;
        [SerializeField] private BallPlusSegEquelSegTutor _figureSum;
        [SerializeField] private GameObject _figureFail;

        private event Action _tutorialClick;

        public event Action TutorialClick
        {
            add
            {
                _tutorialButton.gameObject.SetActive(true);
                _tutorialClick += value;
            }
            remove
            {
                _tutorialClick -= value;
                if (_tutorialClick == null)
                {
                    _tutorialButton.gameObject.SetActive(false);
                }
            }
        }

        public GameObject NewCircleArrowWithHand => Instantiate(_circleArrowWithHand);

        public BallPlusSegEquelSegTutor NumSum => _numSum;

        public GameObject TargetArrow => _targetArrow;

        public GameObject NewLoseRules => Instantiate(_loseRules);

        public GameObject BlackPanel => _blackPanel;

        public GameObject TargetCount => _targetCount;

        public GameObject TargetValue => _targetValue;

        public GameObject TargetPanel => _targetPanel;

        public GameObject CubeReverse => _cubeReverse;

        public BallPlusSegEquelSegTutor CubeSum => _cubeSum;

        public GameObject CubeFail => _cubeFail;

        public BallPlusSegEquelSegTutor ColorSum => _colorSum;

        public GameObject ColorFail => _colorFail;

        public GameObject ColorStart => _colorStart;

        public GameObject FigureStart => _figureStart;

        public GameObject FigureFail => _figureFail;

        public BallPlusSegEquelSegTutor FigureSum => _figureSum;

        private void Awake()
        {
            _tutorialButton.AddListener((eventData) => _tutorialClick?.Invoke());
        }

        private void OnDestroy()
        {
            _tutorialButton.RemoveAllListeners();
        }

        public void SetRulesText(string text)
        {
            _TextArea.SetActive(String.IsNullOrWhiteSpace(text) == false);
            _rulesText.text = text;
        }
    }
}