
namespace AnimationPlayers
{
    using DG.Tweening;
    using System.Threading.Tasks;
    using UnityEngine;

    public class AdditiveAnimationPlayer2D : AdditiveAnimationPlayer
    {
        protected override void AddColor()
        {
            Color newColor = (TargetAnimation.SpriteRenderer.color + TargetAnimation.AdditiveColor) / ColorsDivider;
            TargetAnimation.SpriteRenderer.DOColor(newColor, TargetAnimation.Duration).SetDelay(TargetAnimation.Delay);
        }

        protected override async Task AsyncAddColor()
        {
            Color newColor = (TargetAnimation.SpriteRenderer.color + TargetAnimation.AdditiveColor) / ColorsDivider;
            Tween tween = TargetAnimation.SpriteRenderer.DOColor(newColor, TargetAnimation.Duration).SetDelay(TargetAnimation.Delay);

            await tween.AsyncWaitForCompletion();
        }
    }

}
