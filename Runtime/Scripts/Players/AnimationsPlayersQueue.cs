using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public class AnimationsPlayersQueue : BasePlayer
    {
        [SerializeField] private List<BasePlayer> _players;

        public override void Play(Action onCompleteCallback = null)
        {
            if (_players.Count == 0)
                return;

            Prepare();
            PlayWithCallback(onCompleteCallback).Forget();
        }

        private async UniTask PlayWithCallback(Action onCompleteCallback)
        {
            await AsyncPlay(GetOnDisableCancellationToken());
            onCompleteCallback?.Invoke();
        }

        public override async UniTask AsyncPlay(CancellationToken token)
        {
            if (_players.Count == 0)
                return;

            Prepare();

            CancellationTokenSource source = CombineTokensWithOnDisableToken(token);

            foreach (BasePlayer player in _players)
                await player.AsyncPlay(source.Token);
        }

        public override void Prepare()
        {
            foreach (BasePlayer player in _players)
            {
                player.Prepare();
            }
        }
    }
}
