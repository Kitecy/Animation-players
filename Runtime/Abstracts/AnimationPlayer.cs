using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace AnimationPlayers
{
    public abstract class AnimationPlayer : MonoBehaviour
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

        private async Task AsyncPlayAnimations(List<Animation> animations)
        {
            int minOrder = animations.Min(x => x.Order);
            int maxOrder = animations.Max(x => x.Order);

            for (int order = minOrder; order <= maxOrder; order++)
            {
                List<Animation> animationsWithCurrentOrder = animations.Where(x => x.Order == order).ToList();
                Animation longestAnimation = animationsWithCurrentOrder.OrderBy(animation => animation.Duration).First();

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

        protected abstract Task AsyncPlayPositionAnimation(Animation animation);

        protected abstract Task AsyncPlayColorAnimation(Animation animation);

        protected abstract void PlayPositionAnimation(Animation animation);

        protected abstract void PlayColorAnimation(Animation animation);

        private async Task AsyncPlayRotationAnimation(Animation animation)
        {
            CurrentTransform.rotation = Quaternion.Euler(animation.StartRotation);
            Tween tween = CurrentTransform.DORotate(animation.EndRotation, animation.Duration).SetEase(animation.Ease);

            await tween.AsyncWaitForCompletion();
        }

        private async Task AsyncPlayScaleAnimation(Animation animation)
        {
            CurrentTransform.localScale = animation.StartScale;
            Tween tween = CurrentTransform.DOScale(animation.EndScale, animation.Duration).SetEase(animation.Ease);

            await tween.AsyncWaitForCompletion();
        }

        private void PlayRotationAnimation(Animation animation)
        {
            CurrentTransform.rotation = Quaternion.Euler(animation.StartRotation);
            CurrentTransform.DORotate(animation.EndRotation, animation.Duration).SetEase(animation.Ease);
        }

        private void PlayScaleAnimation(Animation animation)
        {
            CurrentTransform.localScale = animation.StartScale;
            CurrentTransform.DOScale(animation.EndScale, animation.Duration).SetEase(animation.Ease);
        }
    }
}