using DG.Tweening;
using System;
using UnityEngine;

namespace AnimationPlayers
{
    public abstract class SimpleAnimationPlayer : MonoBehaviour
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
                    PlayPostionAnimation();
                    break;

                case Animation.AnimationType.Scale:
                    PlayScaleAnimation();
                    break;

                case Animation.AnimationType.Rotation:
                    PlayRotationAnimation();
                    break;

                case Animation.AnimationType.Color:
                    PlayColorAnimation();
                    break;
            }
        }

        protected abstract void PlayColorAnimation();

        protected abstract void PlayPostionAnimation();

        private void PlayScaleAnimation()
        {
            CurrentTransform.localScale = TargetAnimation.StartScale;
            Tween tween = CurrentTransform.DOScale(TargetAnimation.EndScale, TargetAnimation.Duration).SetEase(TargetAnimation.Ease);
            tween.OnComplete(() => OnAnimationCompleted());
            tween.Play();
        }

        private void PlayRotationAnimation()
        {
            CurrentTransform.rotation = Quaternion.Euler(TargetAnimation.StartRotation);
            Tween tween = CurrentTransform.DORotate(TargetAnimation.EndRotation, TargetAnimation.Duration).SetEase(TargetAnimation.Ease);
            tween.OnComplete(() => OnAnimationCompleted());
            tween.Play();
        }

        protected void OnAnimationCompleted()
        {
            _onAnimationEnded?.Invoke();
            _onAnimationEnded = null;
        }
    }
}