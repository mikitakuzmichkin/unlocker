using UnityEngine;

namespace PuzzleUnlocker.Gameplay.Content
{
    public abstract class AGameplayContent : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        private static readonly int Select = Shader.PropertyToID("_IsSelect");

        public RectTransform RectTransform => _rect;

        public abstract void UpdateValue(object value, ETypeFromContent fromContent);
        
        protected abstract Material Material { get; }

        public void IsSelect(bool value)
        {
            int v = value ? 1 : 0;
            Material.SetInt(Select, v);
        }
    }
}