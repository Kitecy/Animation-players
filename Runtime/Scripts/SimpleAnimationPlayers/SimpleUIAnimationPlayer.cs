namespace AnimationPlayers
{
    using DG.Tweening;
    using System;
    using System.Threading.Tasks;
    using UnityEngine;

    public sealed class SimpleUIAnimationPlayer : SimpleAnimationPlayer, IUIPlayer
    {
        private RectTransform _rectTransform;

        protected override void Awake()
        {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>();
        }

        public override void Play(Action onAnimationEnded = null)
        {
            if (onAnimationEnded != null)
                OnAnimationEnded = onAnimationEnded;

            switch (TargetAnimation.Type)
            {
                case Animation.AnimationType.Position:
                    SetStartPositionValue();
                    _ = AsyncPlayPositionAnimation();
                    break;

                case Animation.AnimationType.Scale:
                    SetStartScaleValue();
                    _ = AsyncPlayScaleAnimation();
                    break;

                case Animation.AnimationType.Rotation:
                    SetStartRotationValue();
                    _ = AsyncPlayRotationAnimation();
                    break;

                case Animation.AnimationType.Color:
                    SetStartColorValue();
                    _ = AsyncPlayColorAnimation();
                    break;

                case Animation.AnimationType.Anchor:
                    SetStartAnchorValue();
                    _ = AsyncPlayAnchorAnimation();
                    break;
            }
        }

        public override async Task AsyncPlay()
        {
            switch (TargetAnimation.Type)
            {
                case Animation.AnimationType.Position:
                    SetStartPositionValue();
                    await AsyncPlayPositionAnimation();
                    break;

                case Animation.AnimationType.Scale:
                    SetStartScaleValue();
                    await AsyncPlayScaleAnimation();
                    break;

                case Animation.AnimationType.Rotation:
                    SetStartRotationValue();
                    await AsyncPlayRotationAnimation();
                    break;

                case Animation.AnimationType.Color:
                    SetStartColorValue();
                    await AsyncPlayColorAnimation();
                    break;

                case Animation.AnimationType.Anchor:
                    SetStartAnchorValue();
                    await AsyncPlayAnchorAnimation();
                    break;
            }
        }

        protected override async Task AsyncPlayColorAnimation()
        {
            Tween tween = TargetAnimation.Graphic.DOColor(TargetAnimation.EndColor, TargetAnimation.Duration)
                .SetEase(TargetAnimation.Ease)
                .SetDelay(TargetAnimation.Delay)
                .SetLoops(TargetAnimation.Loops, TargetAnimation.LoopType)
                .SetAutoKill(TargetAnimation.AutoKill);

            await tween.AsyncWaitForCompletion();
        }

        private void SetStartAnchorValue()
        {
            _rectTransform.anchoredPosition = TargetAnimation.AnchoredStartPosition;
        }

        protected override void SetStartColorValue()
        {
            TargetAnimation.Graphic.color = TargetAnimation.StartColor;
        }

        private async Task AsyncPlayAnchorAnimation()
        {
            _rectTransform.anchoredPosition += TargetAnimation.AnchoredStartPosition;

            SetStartAnchorValue();
            Tween tween = _rectTransform.DOAnchorPos(TargetAnimation.AnchoredEndPosition, TargetAnimation.Duration)
                .SetEase(TargetAnimation.Ease)
                .SetDelay(TargetAnimation.Delay)
                .SetLoops(TargetAnimation.Loops, TargetAnimation.LoopType)
                .SetAutoKill(TargetAnimation.AutoKill);

            await tween.AsyncWaitForCompletion();
        }
    }
}
