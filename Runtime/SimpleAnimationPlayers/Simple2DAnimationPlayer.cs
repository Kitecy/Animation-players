namespace AnimationPlayers
{
    using DG.Tweening;

    public sealed class Simple2DAnimationPlayer : SimpleAnimationPlayer
    {
        protected override void PlayColorAnimation()
        {
            TargetAnimation.SpriteRenderer.color = TargetAnimation.StartColor;
            Tween tween = TargetAnimation.SpriteRenderer.DOColor(TargetAnimation.EndColor, TargetAnimation.Duration).SetEase(TargetAnimation.Ease);
            tween.OnComplete(() => OnAnimationCompleted());
        }

        protected override void PlayPostionAnimation()
        {
            CurrentTransform.position = TargetAnimation.StartPosition;
            Tween tween = CurrentTransform.DOMove(TargetAnimation.EndPosition, TargetAnimation.Duration).SetEase(TargetAnimation.Ease);
            tween.OnComplete(() => OnAnimationCompleted());
        }
    }
}