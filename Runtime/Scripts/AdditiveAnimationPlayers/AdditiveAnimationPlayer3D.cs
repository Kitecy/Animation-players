namespace AnimationPlayers
{
    using DG.Tweening;
    using System.Threading.Tasks;
    using UnityEngine;

    public class AdditiveAnimationPlayer3D : AdditiveAnimationPlayer
    {
        protected override void AddColor()
        {
            Color newColor = (TargetAnimation.Renderer.material.color + TargetAnimation.AdditiveColor) / ColorsDivider;
            TargetAnimation.Renderer.material.DOColor(newColor, TargetAnimation.Duration).SetDelay(TargetAnimation.Delay);
        }

        protected override async Task AsyncAddColor()
        {
            Color newColor = (TargetAnimation.Renderer.material.color + TargetAnimation.AdditiveColor) / ColorsDivider;
            Tween tween = TargetAnimation.Renderer.material.DOColor(newColor, TargetAnimation.Duration).SetDelay(TargetAnimation.Delay);

            await tween.AsyncWaitForCompletion();
        }
    }
}
