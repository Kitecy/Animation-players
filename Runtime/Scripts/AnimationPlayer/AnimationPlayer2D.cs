namespace AnimationPlayers
{
    using DG.Tweening;
    using System.Threading.Tasks;

    public sealed class AnimationPlayer2D : AnimationPlayer
    {
        protected override async Task AsyncPlayColorAnimation(Animation animation)
        {
            SetStartColorValue(animation);
            Tween tween = animation.SpriteRenderer.DOColor(animation.EndColor, animation.Duration)
                .SetEase(animation.Ease)
                .SetDelay(animation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        protected override async Task AsyncPlayPositionAnimation(Animation animation)
        {
            SetStartPositionValue(animation);
            Tween tween = CurrentTransform.DOMove(animation.EndPosition, animation.Duration)
                .SetEase(animation.Ease)
                .SetDelay(animation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        protected override void SetStartColorValue(Animation animation)
        {
            animation.SpriteRenderer.color = animation.StartColor;
        }

        protected override void SetStartPositionValue(Animation animation)
        {
            CurrentTransform.position = animation.StartPosition;
        }
    }
}