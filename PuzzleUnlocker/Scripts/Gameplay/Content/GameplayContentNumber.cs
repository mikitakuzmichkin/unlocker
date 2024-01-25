using System;
using PuzzleUnlocker.Scripts.UI;
using TMPro;
using UnityEngine;

namespace PuzzleUnlocker.Gameplay.Content
{
    public class GameplayContentNumber : AGameplayContent
    {
        [SerializeField] private TextMeshProUGUI _text;
        public override void UpdateValue(object value, ETypeFromContent fromContent)
        {
            if (value is null)
            {
                _text.color = Color.clear;
                return;
            }

            if (value is string s)
            {
                UpdateValue(s); 
                return;
            }
            
            if (value is int i)
            {
                UpdateValue(i, fromContent);
                return;
            }

            throw new ArgumentException();
        }

        protected override Material Material => _text.fontMaterial;

        public void UpdateValue(string value)
        {
            _text.text = value;
            if (value == "+")
            {
                _text.color = PuColor.Blue;
                return;
            }

            if (value == "-")
            {
                _text.color = PuColor.Red;
                return;
            }
            
            throw new ArgumentException("Is not right value. String value can only be \"+\" or \"-\"");
        }
        
        public void UpdateValue(int number, ETypeFromContent fromContent)
        {
            _text.text = number.ToString();

            if (fromContent == ETypeFromContent.Ball)
            {
                if (number > 0)
                {
                    _text.color = PuColor.BlueForBall;
                    return;
                }

                if (number < 0)
                {
                    _text.color = PuColor.RedForBall;
                    return;
                }
                
                _text.color = Color.white;
            }
            else
            {
                if (number > 0)
                {
                    _text.color = new Color32(113,161,253,255);
                    return;
                }

                if (number < 0)
                {
                    _text.color = new Color32(246,96,96,255);
                    return;
                }
                
                _text.color = Color.black;
            }
        }
    }
}