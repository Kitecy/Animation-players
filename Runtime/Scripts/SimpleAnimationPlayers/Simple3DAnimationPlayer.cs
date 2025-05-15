namespace AnimationPlayers
{
    using DG.Tweening;
    using System.Threading.Tasks;

    public sealed class Simple3DAnimationPlayer : SimpleAnimationPlayer
    {
        protected override async Task AsyncPlayColorAnimation()
        {
            Tween tween = TargetAnimation.Renderer.material.DOColor(TargetAnimation.EndColor, TargetAnimation.Duration)
                .SetEase(TargetAnimation.Ease)
                .SetDelay(TargetAnimation.Delay)
                .SetLoops(TargetAnimation.Loops, TargetAnimation.LoopType)
                .SetAutoKill(TargetAnimation.AutoKill);

            await tween.AsyncWaitForCompletion();
        }

        protected override void KillColorTween()
        {
            DOTween.Kill(TargetAnimation.Renderer.material);
        }

        protected override void SetStartColorValue()
        {
            TargetAnimation.Renderer.material.color = TargetAnimation.StartColor;
        }
    }
}