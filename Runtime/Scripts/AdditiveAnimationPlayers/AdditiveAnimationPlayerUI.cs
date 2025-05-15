namespace AnimationPlayers
{
    using DG.Tweening;
    using System.Threading.Tasks;
    using UnityEngine;

    public class AdditiveAnimationPlayerUI : AdditiveAnimationPlayer
    {
        protected override void AddColor()
        {
            Color newColor = (TargetAnimation.Graphic.color + TargetAnimation.AdditiveColor) / ColorsDivider;
            TargetAnimation.Graphic.DOColor(newColor, TargetAnimation.Duration).SetDelay(TargetAnimation.Delay);
        }

        protected override async Task AsyncAddColor()
        {
            Color newColor = (TargetAnimation.Graphic.color + TargetAnimation.AdditiveColor) / ColorsDivider;
            Tween tween = TargetAnimation.Graphic.DOColor(newColor, TargetAnimation.Duration).SetDelay(TargetAnimation.Delay);

            await tween.AsyncWaitForCompletion();
        }
    }
}
