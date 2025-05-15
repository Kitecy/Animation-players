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
                .SetDelay(animation.Delay)
                .SetLoops(animation.Loops, animation.LoopType)
                .SetAutoKill(animation.AutoKill);

            await tween.AsyncWaitForCompletion();
        }

        protected override void SetStartColorValue(Animation animation)
        {
            animation.SpriteRenderer.color = animation.StartColor;
        }
    }
}