namespace AnimationPlayers
{
    using DG.Tweening;
    using System.Threading.Tasks;

    public sealed class Simple2DAnimationPlayer : SimpleAnimationPlayer
    {
        protected override async Task AsyncPlayColorAnimation()
        {
            Tween tween = TargetAnimation.SpriteRenderer.DOColor(TargetAnimation.EndColor, TargetAnimation.Duration)
                .SetEase(TargetAnimation.Ease)
                .SetDelay(TargetAnimation.Delay)
                .SetLoops(TargetAnimation.Loops, TargetAnimation.LoopType)
                .SetAutoKill(TargetAnimation.AutoKill);

            await tween.AsyncWaitForCompletion();
        }

        protected override void KillColorTween()
        {
            DOTween.Kill(TargetAnimation.SpriteRenderer);
        }

        protected override void SetStartColorValue()
        {
            TargetAnimation.SpriteRenderer.color = TargetAnimation.StartColor;
        }
    }
}