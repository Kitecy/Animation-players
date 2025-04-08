namespace AnimationPlayers
{
    using DG.Tweening;
    using System;
    using System.Threading.Tasks;
    using UnityEngine;

    public abstract class SimpleAnimationPlayer : MonoBehaviour, IPlayer
    {
        [SerializeField] protected Animation TargetAnimation;

        protected Transform CurrentTransform;

        private Action _onAnimationEnded;

        protected virtual void Awake()
        {
            CurrentTransform = transform;
        }

        private void OnEnable()
        {
            if (TargetAnimation.PlayOnEnable)
                Play();
        }

        public void Play(Action onAnimationEnded = null)
        {
            if (onAnimationEnded != null)
                _onAnimationEnded = onAnimationEnded;

            switch (TargetAnimation.Type)
            {
                case Animation.AnimationType.Position:
                    _ = AsyncPlayPositionAnimation();
                    break;

                case Animation.AnimationType.Scale:
                    _ = AsyncPlayScaleAnimation();
                    break;

                case Animation.AnimationType.Rotation:
                    _ = AsyncPlayRotationAnimation();
                    break;

                case Animation.AnimationType.Color:
                    _ = AsyncPlayColorAnimation();
                    break;
            }
        }

        public async Task AsyncPlay()
        {
            switch (TargetAnimation.Type)
            {
                case Animation.AnimationType.Position:
                    await AsyncPlayPositionAnimation();
                    break;

                case Animation.AnimationType.Scale:
                    await AsyncPlayScaleAnimation();
                    break;

                case Animation.AnimationType.Rotation:
                    await AsyncPlayRotationAnimation();
                    break;

                case Animation.AnimationType.Color:
                    await AsyncPlayColorAnimation();
                    break;
            }
        }

        public void SetStartValue()
        {
            switch (TargetAnimation.Type)
            {
                case Animation.AnimationType.Position:
                    SetStartPositionValue();
                    break;

                case Animation.AnimationType.Scale:
                    SetStartScaleValue();
                    break;

                case Animation.AnimationType.Rotation:
                    SetStartRotationValue();
                    break;

                case Animation.AnimationType.Color:
                    SetStartColorValue();
                    break;
            }
        }

        protected abstract void SetStartColorValue();

        protected abstract void SetStartPositionValue();

        protected abstract Task AsyncPlayColorAnimation();

        protected abstract Task AsyncPlayPositionAnimation();

        private async Task AsyncPlayScaleAnimation()
        {
            SetStartScaleValue();
            Tween tween = CurrentTransform.DOScale(TargetAnimation.EndScale, TargetAnimation.Duration)
                .SetEase(TargetAnimation.Ease)
                .SetDelay(TargetAnimation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        private async Task AsyncPlayRotationAnimation()
        {
            SetStartRotationValue();
            Tween tween = CurrentTransform.DORotate(TargetAnimation.EndRotation, TargetAnimation.Duration)
                .SetEase(TargetAnimation.Ease)
                .SetDelay(TargetAnimation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        protected void OnAnimationCompleted()
        {
            _onAnimationEnded?.Invoke();
            _onAnimationEnded = null;
        }

        private void SetStartScaleValue()
        {
            CurrentTransform.localScale = TargetAnimation.StartScale;
        }

        private void SetStartRotationValue()
        {
            CurrentTransform.rotation = Quaternion.Euler(TargetAnimation.StartRotation);
        }
    }
}