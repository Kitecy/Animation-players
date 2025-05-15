using UnityEngine;
using UnityEngine.UI;

namespace AnimationPlayers
{
    [System.Serializable]
    public class AdditiveAnimation
    {
        [SerializeField] private Vector3 _additivePosition;
        [SerializeField] private Vector3 _additiveScale;
        [SerializeField] private Vector3 _additiveRotation;
        [SerializeField] private Color _additiveColor = Color.white;

        [SerializeField] private float _duration = 1;
        [SerializeField] private float _delay;
        [SerializeField] private bool _playOnEnable;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Graphic _graphic;

        [SerializeField] private AdditiveAnimationType _type;

        public enum AdditiveAnimationType
        {
            Position,
            Scale,
            Rotation,
            Color
        }

        public Vector3 AdditivePosition => _additivePosition;
        public Vector3 AdditiveScale => _additiveScale;
        public Vector3 AdditiveRotation => _additiveRotation;
        public Color AdditiveColor => _additiveColor;

        public float Duration => _duration;
        public float Delay => _delay;
        public bool PlayOnEnable => _playOnEnable;
        public float TotalDuration => _delay + _duration;

        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Graphic Graphic => _graphic;
        public Renderer Renderer => _renderer;

        public AdditiveAnimationType Type => _type;
    }
}
