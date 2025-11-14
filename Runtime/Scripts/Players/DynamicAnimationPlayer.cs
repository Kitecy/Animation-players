using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public class DynamicAnimationPlayer : BasePlayer
    {
        private const string Error = "You can't start an animation without specifying it first!";

        private Animation _animation;

        public IReadOnlyAnimation CurrentAnimation => _animation;

        public void SetAnimation(Animation animation)
        {
            _animation = animation ?? throw new ArgumentNullException(nameof(animation));
        }

        public override async UniTask AsyncPlay(CancellationToken token)
        {
            if (_animation == null)
            {
                Debug.LogError(Error);
                return;
            }

            CancellationTokenSource source = CombineTokensWithOnDisableToken(token);

            Tween tween = _animation.Convert(this, IsUI, source.Token);
            await tween.Play();
        }

        public override void Play(Action onCompleteCallback = null)
        {
            if (_animation == null)
            {
                Debug.LogError(Error);
                return;
            }

            Tween tween = _animation.Convert(this, IsUI, GetOnDisableCancellationToken());
            tween.Play();
        }

        public override void Prepare()
        {
            _animation.Prepare(this, IsUI);
        }
    }
}
