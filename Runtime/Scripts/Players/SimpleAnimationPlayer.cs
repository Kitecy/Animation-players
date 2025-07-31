using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public class SimpleAnimationPlayer : BasePlayer
    {
        [SerializeField] private Animation _playableAnimation;

        public IReadOnlyAnimation Animation => _playableAnimation;

        public override void Play(Action onCompleteCallback = null)
        {
            Tween tween = PrepareForPlay();
            tween.Play().OnComplete(() => onCompleteCallback?.Invoke());
        }

        public override async UniTask AsyncPlay()
        {
            Tween tween = PrepareForPlay();
            tween.Play();

            await tween.AsyncWaitForCompletion();
        }

        public override void Prepare()
        {
            _playableAnimation.Prepare(this, IsUI);
        }

        private Tween PrepareForPlay()
        {
            Prepare();
            return _playableAnimation.Convert(this, IsUI);
        }
    }
}