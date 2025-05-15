namespace AnimationPlayers
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    [System.Serializable]
    public class Animation
    {
        public enum AnimationType
        {
            Position,
            Scale,
            Rotation,
            Color,
            Anchor
        }

        [SerializeField] private string _name;
        [SerializeField] private int _order;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _delay;
        [SerializeField] private bool _playOnEnable;
        [SerializeField] private bool _autoKill;

        [SerializeField] private LoopType _loopType;
        [SerializeField] private int _loops = 1;

        [SerializeField] private AnimationType _type;

        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _endPosition;

        [SerializeField] private Vector2 _anchoredStartPosition;
        [SerializeField] private Vector2 _anchoredEndPosition;

        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale = Vector3.one;

        [SerializeField] private Vector3 _startRotation;
        [SerializeField] private Vector3 _endRotation;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Graphic _graphic;

        [SerializeField] private Color _startColor = Color.white;
        [SerializeField] private Color _endColor = Color.white;

        [SerializeField] private Ease _ease;

        public string Name => _name;
        public int Order => _order;
        public float Duration => _duration;
        public float Delay => _delay;
        public float TotalDuration => _duration + _delay;
        public bool PlayOnEnable => _playOnEnable;
        public bool AutoKill => _autoKill;

        public LoopType LoopType => _loopType;
        public int Loops => _loops;

        public AnimationType Type => _type;

        public Vector3 StartPosition => _startPosition;
        public Vector3 EndPosition => _endPosition;

        public Vector2 AnchoredStartPosition => _anchoredStartPosition;
        public Vector2 AnchoredEndPosition => _anchoredEndPosition;

        public Vector3 StartScale => _startScale;
        public Vector3 EndScale => _endScale;

        public Vector3 StartRotation => _startRotation;
        public Vector3 EndRotation => _endRotation;

        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        public Renderer Renderer => _renderer;
        public Graphic Graphic => _graphic;

        public Color StartColor => _startColor;
        public Color EndColor => _endColor;

        public Ease Ease => _ease;

        public void SetStartPosition(Vector3 position)
        {
            _startPosition = position;
        }

        public void SetEndPosition(Vector3 position)
        {
            _endPosition = position;
        }
    }
}