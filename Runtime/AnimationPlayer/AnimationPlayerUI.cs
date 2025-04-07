using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

namespace AnimationPlayers
{
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
            animation.Graphic.color = animation.StartColor;
            Tween tween = animation.Graphic.DOColor(animation.EndColor, animation.Duration).SetEase(animation.Ease);

            await tween.AsyncWaitForCompletion();
        }

        protected override async Task AsyncPlayPositionAnimation(Animation animation)
        {
            _rectTransform.position = animation.StartPosition;
            Tween tween = _rectTransform.DOMove(animation.EndPosition, animation.Duration).SetEase(animation.Ease);

            await tween.AsyncWaitForCompletion();
        }

        protected override void PlayColorAnimation(Animation animation)
        {
            animation.Graphic.color = animation.StartColor;
            animation.Graphic.DOColor(animation.EndColor, animation.Duration).SetEase(animation.Ease);
        }

        protected override void PlayPositionAnimation(Animation animation)
        {
            _rectTransform.position = animation.StartPosition;
            _rectTransform.DOMove(animation.EndPosition, animation.Duration).SetEase(animation.Ease);
        }
    }
}