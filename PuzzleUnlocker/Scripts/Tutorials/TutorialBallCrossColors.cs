using System;
using PuzzleUnlocker.Gameplay.Content;
using PuzzleUnlocker.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PuzzleUnlocker.Tutorials
{
    public class TutorialBallCrossColors : MonoBehaviour
    {
        [SerializeField] private Image _r;
        [SerializeField] private Image _g;
        [SerializeField] private Image _b;

        public void UpdateValue(Color color)
        {
            _r.gameObject.SetActive(color.r > 0);
            _g.gameObject.SetActive(color.g > 0);
            _b.gameObject.SetActive(color.b > 0);
        }
    }
}