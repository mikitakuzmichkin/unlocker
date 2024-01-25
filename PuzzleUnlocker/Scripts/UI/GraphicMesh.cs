using UnityEngine;
using UnityEngine.UI;

namespace PuzzleUnlocker.Scripts.UI
{
    [RequireComponent(typeof(MeshRenderer))]
    public class GraphicMesh : Graphic
    {
        private Material _material;
        protected override void Awake()
        {
            _material = GetComponent<MeshRenderer>().material;
            base.Awake();
        }

        public override Color color
        {
            get => base.color;
            set
            {
                _material.SetColor("_Color", value);
                base.color = value;
            }
        }
    }
}