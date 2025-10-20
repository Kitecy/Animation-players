using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public class SimpleAnimationPlayer : BasePlayer
    {
        [SerializeField] private Animation _playableAnimation;

        private Tween _currentTween;

        public IReadOnlyAnimation Animation => _playableAnimation;

        public override void Play(Action onCompleteCallback = null)
        {
            Tween tween = PrepareForPlay();
            tween.Play().OnComplete(() => onCompleteCallback?.Invoke());
        }

        public override async UniTask AsyncPlay()
        {
            if (OnDisableToken.IsCancellationRequested)
                return;

            _currentTween = PrepareForPlay();
            _currentTween.Play();

            await _currentTween.AsyncWaitForCompletion();
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

        public override void Stop()
        {
            _currentTween.Kill();
        }
    }
}