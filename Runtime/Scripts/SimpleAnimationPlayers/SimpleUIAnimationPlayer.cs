namespace AnimationPlayers
{
    using DG.Tweening;
    using System.Threading.Tasks;
    using UnityEngine;

    public sealed class SimpleUIAnimationPlayer : SimpleAnimationPlayer
    {
        private RectTransform _transform;

        protected override void Awake()
        {
            base.Awake();

            _transform = GetComponent<RectTransform>();
        }

        protected override async Task AsyncPlayColorAnimation()
        {
            SetStartColorValue();
            Tween tween = TargetAnimation.Graphic.DOColor(TargetAnimation.EndColor, TargetAnimation.Duration)
                .SetEase(TargetAnimation.Ease)
                .SetDelay(TargetAnimation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        protected override async Task AsyncPlayPositionAnimation()
        {
            SetStartPositionValue();
            Tween tween = _transform.DOMove(TargetAnimation.EndPosition, TargetAnimation.Duration)
                .SetEase(TargetAnimation.Ease)
                .SetDelay(TargetAnimation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        protected override void SetStartColorValue()
        {
            TargetAnimation.Graphic.color = TargetAnimation.StartColor;
        }

        protected override void SetStartPositionValue()
        {
            _transform.position = TargetAnimation.StartPosition;
        }
    }

}
