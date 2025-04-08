namespace AnimationPlayers
{
    using DG.Tweening;

    public sealed class Simple3DAnimationPlayer : SimpleAnimationPlayer
    {
        protected override void PlayColorAnimation()
        {
            TargetAnimation.MeshRenderer.material.color = TargetAnimation.StartColor;
            Tween tween = TargetAnimation.MeshRenderer.material.DOColor(TargetAnimation.EndColor, TargetAnimation.Duration).SetEase(TargetAnimation.Ease);
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