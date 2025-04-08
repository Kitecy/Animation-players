namespace AnimationPlayers
{
    using DG.Tweening;
    using System.Threading.Tasks;

    public sealed class AnimationPlayer3D : AnimationPlayer
    {
        protected override async Task AsyncPlayColorAnimation(Animation animation)
        {
            animation.MeshRenderer.material.color = animation.StartColor;
            Tween tween = animation.MeshRenderer.material.DOColor(animation.EndColor, animation.Duration).SetEase(animation.Ease);

            await tween.AsyncWaitForCompletion();
        }

        protected override async Task AsyncPlayPositionAnimation(Animation animation)
        {
            CurrentTransform.position = animation.StartPosition;
            Tween tween = CurrentTransform.DOMove(animation.EndPosition, animation.Duration).SetEase(animation.Ease);

            await tween.AsyncWaitForCompletion();
        }

        protected override void PlayColorAnimation(Animation animation)
        {
            animation.MeshRenderer.material.color = animation.StartColor;
            animation.MeshRenderer.material.DOColor(animation.EndColor, animation.Duration).SetEase(animation.Ease);
        }

        protected override void PlayPositionAnimation(Animation animation)
        {
            CurrentTransform.position = animation.StartPosition;
            CurrentTransform.DOMove(animation.EndPosition, animation.Duration).SetEase(animation.Ease);
        }
    }
}
