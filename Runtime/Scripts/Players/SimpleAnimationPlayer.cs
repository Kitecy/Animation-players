using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
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
            Tween tween = PrepareForPlay(GetOnDisableCancellationToken());
            tween.Play().OnComplete(() => onCompleteCallback?.Invoke());
        }

        public override async UniTask AsyncPlay(CancellationToken token)
        {
            _currentTween = PrepareForPlay(token);
            await _currentTween.Play();
        }

        public override void Prepare()
        {
            _playableAnimation.Prepare(this, IsUI);
        }

        private Tween PrepareForPlay(CancellationToken token)
        {
            Prepare();
            CancellationTokenSource source = CombineTokensWithOnDisableToken(token);
            return _playableAnimation.Convert(this, IsUI, source.Token);
        }
    }
}