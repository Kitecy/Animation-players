namespace AnimationPlayers
{
    using DG.Tweening;
    using System.Threading.Tasks;

    public sealed class Simple3DAnimationPlayer : SimpleAnimationPlayer
    {
        protected override async Task AsyncPlayColorAnimation()
        {
            SetStartColorValue();
            Tween tween = TargetAnimation.Renderer.material.DOColor(TargetAnimation.EndColor, TargetAnimation.Duration)
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
            TargetAnimation.Renderer.material.color = TargetAnimation.StartColor;
        }

        protected override void SetStartPositionValue()
        {
            CurrentTransform.position = TargetAnimation.StartPosition;
        }
    }
}