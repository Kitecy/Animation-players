namespace AnimationPlayers
{
    using DG.Tweening;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UnityEngine;

    public abstract class AnimationPlayer : MonoBehaviour, IPlayer
    {
        [SerializeField] protected List<Animation> Animations = new();

        private Action _onAnimationEnded;

        protected Transform CurrentTransform;

        protected virtual void Awake()
        {
            CurrentTransform = transform;
        }

        private void OnEnable()
        {
            List<Animation> animations = Animations.Where(animation => animation.PlayOnEnable == true).ToList();

            if (animations.Count > 0)
                _ = AsyncPlayAnimations(animations);
        }

        public void Play(string animationName, Action onAnimationEnded = null)
        {
            Animation animation = Animations.Find(anim => anim.Name == animationName);

            if (onAnimationEnded != null)
                _onAnimationEnded = onAnimationEnded;

            _ = AsyncPlayAnimations(new List<Animation>() { animation });
        }

        public void Play(int order, Action onAnimationEnded = null)
        {
            if (onAnimationEnded != null)
                _onAnimationEnded = onAnimationEnded;

            List<Animation> animations = Animations.Where(animation => animation.Order == order).ToList();
            _ = AsyncPlayAnimations(animations);
        }

        public void PlayAll(Action onAnimationEnded = null)
        {
            if (onAnimationEnded != null)
                _onAnimationEnded = onAnimationEnded;

            _ = AsyncPlayAnimations(Animations);
        }

        public async Task AsyncPlay(string animationName)
        {
            Animation animation = Animations.Find(anim => anim.Name == animationName);
            await AsyncPlayAnimations(new List<Animation>() { animation });
        }

        public async Task AsyncPlayAll()
        {
            await AsyncPlayAnimations(Animations);
        }

        public void SetStartValue()
        {
            Animation firstAnimation = Animations.OrderBy(anim => anim.Order).First();

            switch (firstAnimation.Type)
            {
                case Animation.AnimationType.Position:
                    SetStartPositionValue(firstAnimation);
                    break;

                case Animation.AnimationType.Rotation:
                    SetStartRotationValue(firstAnimation);
                    break;

                case Animation.AnimationType.Scale:
                    SetStartScaleValue(firstAnimation);
                    break;

                case Animation.AnimationType.Color:
                    SetStartColorValue(firstAnimation);
                    break;
            }
        }

        private async Task AsyncPlayAnimations(List<Animation> animations)
        {
            int minOrder = animations.Min(x => x.Order);
            int maxOrder = animations.Max(x => x.Order);

            for (int order = minOrder; order <= maxOrder; order++)
            {
                List<Animation> animationsWithCurrentOrder = animations.Where(x => x.Order == order).ToList();
                Animation longestAnimation = animationsWithCurrentOrder.OrderBy(animation => animation.TotalDuration).First();

                for (int i = 0; i < animationsWithCurrentOrder.Count; i++)
                    _ = AsyncProcessAnimation(animationsWithCurrentOrder[i]);

                await AsyncProcessAnimation(longestAnimation);
            }

            _onAnimationEnded?.Invoke();
            _onAnimationEnded = null;
        }

        private async Task AsyncProcessAnimation(Animation animation)
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
            }
        }

        protected abstract void SetStartColorValue(Animation animation);

        protected abstract void SetStartPositionValue(Animation animation);

        protected abstract Task AsyncPlayPositionAnimation(Animation animation);

        protected abstract Task AsyncPlayColorAnimation(Animation animation);

        private async Task AsyncPlayRotationAnimation(Animation animation)
        {
            SetStartRotationValue(animation);
            Tween tween = CurrentTransform.DORotate(animation.EndRotation, animation.Duration)
                .SetEase(animation.Ease)
                .SetDelay(animation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        private async Task AsyncPlayScaleAnimation(Animation animation)
        {
            SetStartScaleValue(animation);
            Tween tween = CurrentTransform.DOScale(animation.EndScale, animation.Duration)
                .SetEase(animation.Ease)
                .SetDelay(animation.Delay);

            await tween.AsyncWaitForCompletion();
        }

        private void SetStartRotationValue(Animation animation)
        {
            CurrentTransform.rotation = Quaternion.Euler(animation.StartRotation);
        }

        private void SetStartScaleValue(Animation animation)
        {
            CurrentTransform.localScale = animation.StartScale;
        }
    }
}