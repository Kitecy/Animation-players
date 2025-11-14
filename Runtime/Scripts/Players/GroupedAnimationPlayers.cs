using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace AnimationPlayers.Players
{
    [DisallowMultipleComponent]
    public class GroupedAnimationPlayers : BasePlayer
    {
        private readonly string _error = "You cannot specify this component in this field!";

        [SerializeField] private BasePlayer _player;
        [SerializeField] private List<BasePlayer> _players;
        [SerializeField] private float _interval = 0.125f;
        [SerializeField] private float _delay = 0;

        private void OnValidate()
        {
            if (_player == this)
            {
                _player = null;
                Debug.LogError(_error);
            }
        }

        public override void Play(Action onCompleteCallback = null)
        {
            if (_players.Count == 0)
                return;

            PlayWithCallback(onCompleteCallback).Forget();
        }

        private async UniTaskVoid PlayWithCallback(Action onCompleteCallback)
        {
            if (_player != null && _player.didStart == false)
                await UniTask.Yield();

            await AsyncPlay(GetOnDisableCancellationToken());
            onCompleteCallback?.Invoke();
        }

        public override async UniTask AsyncPlay(CancellationToken token)
        {
            if (_players.Count == 0)
                return;

            Prepare();

            CancellationTokenSource source = CombineTokensWithOnDisableToken(token);

            TimeSpan interval = TimeSpan.FromSeconds(_interval);

            if (_delay > 0)
            {
                TimeSpan delay = TimeSpan.FromSeconds(_delay);
                await UniTask.Delay(delay, cancellationToken: source.Token);
            }

            if (_player != null)
            {
                await _player.AsyncPlay(source.Token);
                await UniTask.Delay(interval, cancellationToken: source.Token);
            }


            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].AsyncPlay(source.Token).Forget();

                if (i < _players.Count - 1)
                    await UniTask.Delay(interval, cancellationToken: source.Token);
            }
        }

        public void SetPlayersFromChildren()
        {
            List<BasePlayer> players = GetComponentsInChildren<BasePlayer>().ToList();
            List<BasePlayer> ownPlayers = players.FindAll(x => x.gameObject == gameObject);

            foreach (BasePlayer player in ownPlayers)
                players.Remove(player);

            _players = players;
        }

        public override void Prepare()
        {
            if (_player != null)
                _player.Prepare();

            foreach (BasePlayer player in _players)
                player.Prepare();
        }
    }
}