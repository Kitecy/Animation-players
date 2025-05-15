namespace AnimationPlayers
{
    using DG.Tweening;
    using System.Threading.Tasks;
    using UnityEngine;

    public sealed class AnimationPlayerUI : AnimationPlayer, IUIPlayer
    {
        private RectTransform _rectTransform;

        protected override void Awake()
        {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>();
        }

        protected override async Task AsyncProcessAnimation(Animation animation)
        {
            switch (animation.Type)
            {
                case Animation.AnimationType.Position:
                    await AsyncPlayPositionAnimation(animation);
                    break;

                case Animation.AnimationType.Rotation:
                    await AsyncPlayRotationAnimation(animation);
                    break;

                case Animation.AnimationType.Scale:
                    await AsyncPlayScaleAnimation(animation);
                    break;

                case Animation.AnimationType.Color:
                    await AsyncPlayColorAnimation(animation);
                    break;

                case Animation.AnimationType.Anchor:
                    await AsyncPlayAnchorAnimation(animation);
                    break;
            }
        }

        protected override async Task AsyncPlayColorAnimation(Animation animation)
        {
            SetStartColorValue(animation);
            Tween tween = animation.Graphic.DOColor(animation.EndColor, animation.Duration)
                .SetEase(animation.Ease)
                .SetDelay(animation.Delay)
                .SetLoops(animation.Loops, animation.LoopType)
                .SetAutoKill(animation.AutoKill);

            await tween.AsyncWaitForCompletion();
        }

        private async Task AsyncPlayAnchorAnimation(Animation animation)
        {
            SetStartAnchorValue(animation);
            Tween tween = _rectTransform.DOAnchorPos(animation.AnchoredEndPosition, animation.Duration)
                .SetEase(animation.Ease)
                .SetDelay(animation.Delay)
                .SetLoops(animation.Loops, animation.LoopType)
                .SetAutoKill(animation.AutoKill);

            await tween.AsyncWaitForCompletion();
        }

        protected override void SetStartColorValue(Animation animation)
        {
            animation.Graphic.color = animation.StartColor;
        }

        private void SetStartAnchorValue(Animation animation)
        {
            _rectTransform.anchoredPosition = animation.AnchoredStartPosition;
        }
    }
}