using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace AnimationPlayers.Players
{
    [Serializable]
    public class Animation : IReadOnlyAnimation
    {
        public const float MinDurationValue = 0.001f;

        public enum Type
        {
            Position,
            Rotation,
            Scale,
            Color,
            Fade,
            Anchor
        }

        [SerializeField] private string _name;
        [SerializeField] private int _order;
        [SerializeField] private float _duration = 1f;
        [SerializeField] private float _delay;

        [SerializeField] private bool _isEternalLoop = false;
        [SerializeField] private int _loops = 1;
        [SerializeField] private LoopType _loopType;

        [SerializeField] private Ease _ease = Ease.OutBack;
        [SerializeField] private Type _type;

        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private Vector3 _endPosition = Vector3.one;

        [SerializeField] private Vector3 _startRotation;
        [SerializeField] private Vector3 _endRotation;

        [SerializeField] private Vector3 _startScale;
        [SerializeField] private Vector3 _endScale = Vector3.one;

        [SerializeField] private Color _startColor = new Color(1, 1, 1, 0);
        [SerializeField] private Color _endColor = Color.white;

        [SerializeField] private Renderer _renderer;
        [SerializeField] private Graphic _graphic;

        [SerializeField] private float _startFade = 0;
        [SerializeField] private float _endFade = 1;

        [SerializeField] private Vector2 _startAnchoredPosition;
        [SerializeField] private Vector2 _endAnchoredPosition;

        public string Name => _name;
        public int Order => _order;

        public float Duration => _duration;
        public float Delay => _delay;

        public bool IsEternalLoop => _isEternalLoop;
        public int Loops => _loops;
        public LoopType LoopType => _loopType;

        public Ease Ease => _ease;
        public Type Sort => _type;

        public Vector3 StartPosition => _startPosition;
        public Vector3 EndPosition => _endPosition;

        public Vector3 StartRotation => _startRotation;
        public Vector3 EndRotation => _endRotation;

        public Vector3 StartScale => _startScale;
        public Vector3 EndScale => _endScale;

        public Color StartColor => _startColor;
        public Color EndColor => _endColor;

        public Renderer Renderer => _renderer;
        public Graphic Graphic => _graphic;

        public float StartFade => _startFade;
        public float EndFade => _endFade;

        public Vector2 StartAnchorPosition => _startAnchoredPosition;
        public Vector2 EndAnchorPosition => _endAnchoredPosition;

        public float TotalDuration => _duration + _delay;

        public void SetName(string value) =>
            _name = value;

        public void SetOrder(int value) =>
            _order = value;

        public void SetDuration(float value)
        {
            if (value < MinDurationValue)
                value = MinDurationValue;

            _duration = value;
        }

        public void SetDelay(float value)
        {
            if (value < 0)
                value = 0;

            _delay = value;
        }

        public void SetIsEternalLoop(bool value) =>
            _isEternalLoop = value;

        public void SetLoops(int value) =>
            _loops = value;

        public void SetLoopType(LoopType value) =>
            _loopType = value;

        public void SetEase(Ease value) =>
            _ease = value;

        public void SetType(Type value) =>
            _type = value;

        public void SetStartPosition(Vector3 value) =>
            _startPosition = value;

        public void SetEndPosition(Vector3 value) =>
            _endPosition = value;

        public void SetStartRotation(Vector3 value) =>
            _startRotation = value;

        public void SetEndRotation(Vector3 value) =>
            _endRotation = value;

        public void SetStartScale(Vector3 value) =>
            _startScale = value;

        public void SetEndScale(Vector3 value) =>
            _endScale = value;

        public void SetStartColor(Color value) =>
            _startColor = value;

        public void SetEndColor(Color value) =>
            _endColor = value;

        public void SetRenderer(Renderer value) =>
            _renderer = value;

        public void SetGraphic(Graphic value) =>
            _graphic = value;

        public void SetStartFade(float value)
        {
            value = Mathf.Clamp01(value);
            _startFade = value;
        }

        public void SetEndFade(float value)
        {
            value = Mathf.Clamp01(value);
            _endFade = value;
        }

        public void SetStartAnchoredPosition(Vector2 value) =>
            _startAnchoredPosition = value;

        public void SetEndAnchoredPosition(Vector2 value) =>
            _endAnchoredPosition = value;
    }
}
