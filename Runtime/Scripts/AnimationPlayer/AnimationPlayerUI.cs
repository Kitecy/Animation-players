namespace AnimationPlayers
{
    using DG.Tweening;
    using System.Threading.Tasks;
    using UnityEngine;

    public sealed class AnimationPlayerUI : AnimationPlayer
    {
        private RectTransform _rectTransform;

        protected override void Awake()
        {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>();
        }

        protected override async Task AsyncPlayColorAnimation(Animation animation)
        {
            SetStartColorValue(animation);
            Tween tween = animation.Graphic.DOColor(animation.EndColor, animation.Duration)
                .SetEase(animation.Ease)
                .SetDelay(animation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        protected override async Task AsyncPlayPositionAnimation(Animation animation)
        {
            SetStartPositionValue(animation);
            Tween tween = _rectTransform.DOMove(animation.EndPosition, animation.Duration)
                .SetEase(animation.Ease)
                .SetDelay(animation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        protected override void SetStartPositionValue(Animation animation)
        {
            _rectTransform.position = animation.StartPosition;
        }

        protected override void SetStartColorValue(Animation animation)
        {
            animation.Graphic.color = animation.StartColor;
        }
    }
}