using System;
using System.Collections;
using PuzzleUnlocker.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

namespace PuzzleUnlocker.Gameplay.Content
{
    public class GameplayContentColor : AGameplayContent
    {
        [SerializeField] private Image _r;
        [SerializeField] private Image _g;
        [SerializeField] private Image _b;
        [SerializeField] private Graphic _color;
        
        private void Awake()
        {
            if (_color != null && _color.material != null)
            {
                _color.material = Instantiate(_color.material);
            }
        }
        
        public override void UpdateValue(object value, ETypeFromContent fromContent)
        {
            if (value is null)
            {
                _r.sprite = _g.sprite = _b.sprite = null;
                _r.color = _g.color = _b.color = Color.clear;
                if (_color != null)
                {
                    _color.color = Color.clear;
                }

                return;
            }

            if (value is Color == false)
            {
                throw new ArgumentException();
            }

            var color = (Color) value;
            _r.color = color.r > 0 ? PuColor.Red : Color.clear;
            _g.color = color.g > 0 ? PuColor.Green : Color.clear;
            _b.color = color.b > 0 ? PuColor.Blue : Color.clear;
            if (_color != null)
            {
                _color.color = PuColor.PuColorFromColor(color);
            }
        }

        protected override Material Material => _color.material;
    }
}