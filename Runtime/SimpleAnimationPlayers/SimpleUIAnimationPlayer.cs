using DG.Tweening;
using UnityEngine;

namespace AnimationPlayers
{
    public sealed class SimpleUIAnimationPlayer : SimpleAnimationPlayer
    {
        private RectTransform _transform;

        protected override void Awake()
        {
            base.Awake();

            _transform = GetComponent<RectTransform>();
        }

        protected override void PlayColorAnimation()
        {
            TargetAnimation.Graphic.color = TargetAnimation.StartColor;
            Tween tween = TargetAnimation.Graphic.DOColor(TargetAnimation.EndColor, TargetAnimation.Duration).SetEase(TargetAnimation.Ease);
            tween.OnComplete(() => OnAnimationCompleted());
        }

        protected override void PlayPostionAnimation()
        {
            _transform.position = TargetAnimation.StartPosition;
            Tween tween = _transform.DOMove(TargetAnimation.EndPosition, TargetAnimation.Duration).SetEase(TargetAnimation.Ease);
            tween.OnComplete(() => OnAnimationCompleted());
        }
    }

}
