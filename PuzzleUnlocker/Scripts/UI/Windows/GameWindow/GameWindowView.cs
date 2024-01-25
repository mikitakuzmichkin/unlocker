using System;
using Dainty.UI;
using Dainty.UI.WindowBase;
using DG.Tweening;
using PuzzleUnlocker.Gameplay.Content;
using PuzzleUnlocker.Gameplay.Ring;
using PuzzleUnlocker.Gameplay.Segment;
using PuzzleUnlocker.Scripts.Gameplay.Ball;
using PuzzleUnlocker.Tutorials;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleUnlocker.UI.Windows
{
    public class GameWindowView : AWindowView
    {
        [Header("Top UI")] [SerializeField] private Button _menuButton;
        [SerializeField] private Button _restartButton;

        [Space] [SerializeField] private TMP_Text _startText;
        [SerializeField] private Transform _targetUIContainer;
        [SerializeField] private TextMeshProUGUI _targetComboText;
        [SerializeField] private RingViewBase _ringVariant;
        [SerializeField] private RectTransform _gameArea;
        [SerializeField] private TextMeshProUGUI _levelText;

        [Space] [SerializeField] private SafeArea _safeArea;
        [SerializeField] private RectTransform _bannerArea;

        [Space] [SerializeField] private TutorialView _tutorialView;

        [Header("Ring Params")] [SerializeField]
        private float _bigRadius = 25;

        [SerializeField] private float _smallRadius = 20;
        [SerializeField] private int _blur = 25;
        [SerializeField] private int _space;

        private float _bannerAreaHeight;
        private float _bannerAreaVertPos;

        private Tween _startTextTween;

        public RingViewBase Ring { get; private set; }

        public BallView Ball { get; private set; }
        public TutorialView TutorialView => _tutorialView;

        public event Action MenuButton;
        public event Action RestartButton;

#if DEV
        private LongPressButton _debugWinButton;

        public delegate void DebugWinButtonDelegate(bool longPress);

        public event DebugWinButtonDelegate DebugWinButton;
#endif

        private void Awake()
        {
            _bannerAreaHeight = _bannerArea.rect.height;
            _bannerAreaVertPos = _bannerArea.anchoredPosition.y;

#if DEV
            _levelText.raycastTarget = true;
            _debugWinButton = _levelText.gameObject.AddComponent<LongPressButton>();
#endif
        }

        private void Start()
        {
            _safeArea.Changed += OnSafeAreaChanged;
        }

        protected override void OnInitialized()
        {
            OnSafeAreaChanged();
        }

        protected override void OnSubscribe()
        {
            RingInput.Instance.enabled = true;
            _menuButton.onClick.AddListener(() => MenuButton?.Invoke());
            _restartButton.onClick.AddListener(() => RestartButton?.Invoke());

#if DEV
            _debugWinButton.onClick.AddListener(() => DebugWinButton?.Invoke(false));
            _debugWinButton.onLongPress.AddListener(() => DebugWinButton?.Invoke(true));
#endif
        }

        protected override void OnUnSubscribe()
        {
            RingInput.Instance.enabled = false;
            _menuButton.onClick.RemoveAllListeners();
            _restartButton.onClick.RemoveAllListeners();

#if DEV
            _debugWinButton.onClick.RemoveAllListeners();
            _debugWinButton.onLongPress.RemoveAllListeners();
#endif
        }

        public void ShowStartText()
        {
            _startText.enabled = true;
            Ball.gameObject.SetActive(false);

            if (_startTextTween != null)
            {
                if (_startTextTween.IsPlaying())
                {
                    return;
                }

                _startTextTween.Kill();
            }

            var startTextTransform = _startText.transform;
            var scaleOrigin = startTextTransform.localScale;

            _startTextTween = DOTween.Sequence()
                .Append(startTextTransform.DOScale(scaleOrigin * 0.9f, 0.5f))
                .Append(startTextTransform.DOScale(scaleOrigin, 0.5f))
                .SetLoops(-1)
                .OnKill(() => startTextTransform.localScale = scaleOrigin);
        }

        public void HideStartText()
        {
            if (_startTextTween == null)
            {
                return;
            }

            _startText.enabled = false;
            Ball.gameObject.SetActive(true);

            _startTextTween.Kill();
            _startTextTween = null;
        }

        public RingViewBase CreateRing(SegmentView segmentView, AGameplayContent content, int segmentsCount)
        {
            //transform.parent.GetComponent<CanvasScaler>().referenceResolution
            if (_ringVariant != null)
            {
                _bigRadius = _ringVariant.OuterRadius;
                _smallRadius = _ringVariant.InnerRadiusForBaseSegment;
            }

            var ringGenerator = new RingGenerator(segmentView, segmentsCount, _bigRadius, _smallRadius, _blur, _space,
                content);
            Ring = ringGenerator.Create();

            if (_ringVariant != null)
            {
                Ring.transform.SetParent(_ringVariant.transform.parent, false);
                Ring.transform.localPosition = _ringVariant.transform.localPosition;
                var pos = _gameArea.transform.position;
                pos.z = _ringVariant.transform.position.z;
                _ringVariant.transform.parent.position = pos;
                _ringVariant.gameObject.SetActive(false);
            }

            return Ring;
        }

        public BallView CreateBall(AGameplayContent content = null, BallView ball = null)
        {
            var ballGenerator = new BallGenerator();
            var ringTransform = _ringVariant.transform;
            Ball = ballGenerator.Create(ringTransform.position, content, ball, ringTransform.parent);

            return Ball;
        }

        public void CreateTarget(GameObject content, object value, int count)
        {
            content = Instantiate(content, _targetUIContainer, false);
            content.GetComponentInChildren<AGameplayContent>().UpdateValue(value, ETypeFromContent.Target);
            content.transform.localScale = Vector3.one;
            _targetComboText.text = count + "x";
        }

        public void SetLevelText(string text)
        {
            _levelText.text = text;
        }

        public void ReleaseLevel()
        {
            if (Ring != null)
            {
                Destroy(Ring.gameObject);
            }

            if (Ball != null)
            {
                Destroy(Ball.gameObject);
            }

            for (var i = _targetUIContainer.childCount - 1; i >= 0; i--)
            {
                var child = _targetUIContainer.GetChild(i).gameObject;
                Destroy(child);
            }
        }

        private void OnSafeAreaChanged()
        {
            var bottomOffset = UiRoot.GetSafeArea().y;

            var pos = _bannerArea.anchoredPosition;
            pos.y = _bannerAreaVertPos - bottomOffset;
            _bannerArea.anchoredPosition = pos;

            var size = _bannerArea.sizeDelta;
            size.y = _bannerAreaHeight + bottomOffset;
            _bannerArea.sizeDelta = size;
        }

#if DEV
        public void SetDebugWinButtonEnabled(bool isEnabled)
        {
            _levelText.raycastTarget = isEnabled;
            _debugWinButton.enabled = isEnabled;
        }
#endif

#if UNITY_EDITOR
        [Space] [SerializeField] private bool _useLevelFromEditor;
        [SerializeField] private int _level;
        public int Level => _level;
        public bool IsUseLevelFromEditor => _useLevelFromEditor;
#endif
    }
}