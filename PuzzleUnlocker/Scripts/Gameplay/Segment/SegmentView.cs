using PuzzleUnlocker.Gameplay.Content;
using PuzzleUnlocker.Gameplay.Model;
using UnityEngine;
using Object = System.Object;

namespace PuzzleUnlocker.Gameplay.Segment
{
    public class SegmentView : MonoBehaviour
    {
        [SerializeField] private AGameplayContent _content;

        [SerializeField] public float InnerRadius;
        [SerializeField] public float OuterRadius;
        
        public AGameplayContent Content
        {
            get => _content;
            set => _content = value;
        }
    }
}