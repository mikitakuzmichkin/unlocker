using System;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleUnlocker.Gameplay.Content
{
    public class GameplayContentCube : AGameplayContent
    {
        [SerializeField] private Image _image;

        [Header("Colors")] [SerializeField] private Color _colorInSegment = Color.black;
        [SerializeField] private Color _colorInTarget = Color.black;
        [SerializeField] private Color _colorInBall = Color.white;

        [Space] [SerializeField] private Sprite[] _sprites;

        private void Awake()
        {
            _image.material = Instantiate(_image.material);
        }

        public override void UpdateValue(object value, ETypeFromContent fromContent)
        {
            if (value is null)
            {
                _image.sprite = null;
                _image.color = Color.clear;
                return;
            }

            if (value is int == false)
            {
                throw new ArgumentException();
            }

            int number = (int) value;
            _image.sprite = _sprites[number - 1];

            _image.color = fromContent switch
            {
                ETypeFromContent.Ball => _colorInBall,
                ETypeFromContent.Target => _colorInTarget,
                ETypeFromContent.Segment => _colorInSegment,
                _ => throw new ArgumentOutOfRangeException(nameof(fromContent), fromContent, null)
            };
        }

        protected override Material Material => _image.material;
    }
}