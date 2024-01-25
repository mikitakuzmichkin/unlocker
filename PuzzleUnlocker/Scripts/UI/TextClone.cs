using TMPro;
using UnityEngine;

namespace PuzzleUnlocker.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextClone : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMesh;

        private TextMeshProUGUI _cloneText;

        private void Awake()
        {
            _cloneText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (_cloneText.text != _textMesh.text)
            {
                _cloneText.text = _textMesh.text;
            }
        }
    }
}