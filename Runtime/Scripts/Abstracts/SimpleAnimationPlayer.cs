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

        protected Action OnAnimationEnded;

        protected virtual void Awake()
        {
            CurrentTransform = transform;
        }

        private void OnEnable()
        {
            if (TargetAnimation.PlayOnEnable)
                Play();
        }

        private void OnDisable()
        {
            KillTweens();
        }

        private void KillTweens()
        {
            switch (TargetAnimation.Type)
            {
                case Animation.AnimationType.Position:
                case Animation.AnimationType.Rotation:
                case Animation.AnimationType.Scale:
                    DOTween.Kill(CurrentTransform);
                    break;

                case Animation.AnimationType.Color:
                    KillColorTween();
                    break;

                case Animation.AnimationType.Anchor:
                    DOTween.Kill(CurrentTransform as RectTransform);
                    break;
            }
        }

        public virtual void Play(Action onAnimationEnded = null)
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
            }
        }

        public virtual async Task AsyncPlay()
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
            }
        }

        public virtual void SetStartValue()
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

        protected abstract void KillColorTween();

        protected abstract void SetStartColorValue();

        protected abstract Task AsyncPlayColorAnimation();

        protected async Task AsyncPlayPositionAnimation()
        {
            Tween tween = CurrentTransform.DOMove(TargetAnimation.EndPosition, TargetAnimation.Duration)
                .SetEase(TargetAnimation.Ease)
                .SetDelay(TargetAnimation.Delay)
                .SetLoops(TargetAnimation.Loops, TargetAnimation.LoopType)
                .SetAutoKill(TargetAnimation.AutoKill);

            await tween.AsyncWaitForCompletion();
        }

        protected async Task AsyncPlayScaleAnimation()
        {
            Tween tween = CurrentTransform.DOScale(TargetAnimation.EndScale, TargetAnimation.Duration)
                .SetEase(TargetAnimation.Ease)
                .SetDelay(TargetAnimation.Delay)
                .SetLoops(TargetAnimation.Loops, TargetAnimation.LoopType)
                .SetAutoKill(TargetAnimation.AutoKill);

            await tween.AsyncWaitForCompletion();
        }

        protected async Task AsyncPlayRotationAnimation()
        {
            Tween tween = CurrentTransform.DORotate(TargetAnimation.EndRotation, TargetAnimation.Duration)
                .SetEase(TargetAnimation.Ease)
                .SetDelay(TargetAnimation.Delay)
                .SetLoops(TargetAnimation.Loops, TargetAnimation.LoopType)
                .SetAutoKill(TargetAnimation.AutoKill);

            await tween.AsyncWaitForCompletion();
        }

        protected void OnAnimationCompleted()
        {
            OnAnimationEnded?.Invoke();
            OnAnimationEnded = null;
        }

        protected void SetStartPositionValue()
        {
            transform.position = TargetAnimation.StartPosition;
        }

        protected void SetStartScaleValue()
        {
            CurrentTransform.localScale = TargetAnimation.StartScale;
        }

        protected void SetStartRotationValue()
        {
            CurrentTransform.rotation = Quaternion.Euler(TargetAnimation.StartRotation);
        }
    }
}