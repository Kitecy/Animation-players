namespace AnimationPlayers
{
    using DG.Tweening;
    using System.Threading.Tasks;

    public sealed class Simple2DAnimationPlayer : SimpleAnimationPlayer
    {
        protected override async Task AsyncPlayColorAnimation()
        {
            SetStartColorValue();
            Tween tween = TargetAnimation.SpriteRenderer.DOColor(TargetAnimation.EndColor, TargetAnimation.Duration)
                .SetEase(TargetAnimation.Ease)
                .SetDelay(TargetAnimation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        protected override async Task AsyncPlayPositionAnimation()
        {
            SetStartPositionValue();
            Tween tween = CurrentTransform.DOMove(TargetAnimation.EndPosition, TargetAnimation.Duration)
                .SetEase(TargetAnimation.Ease)
                .SetDelay(TargetAnimation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        protected override void SetStartColorValue()
        {
            TargetAnimation.SpriteRenderer.color = TargetAnimation.StartColor;
        }

        protected override void SetStartPositionValue()
        {
            CurrentTransform.position = TargetAnimation.StartPosition;
        }
    }
}