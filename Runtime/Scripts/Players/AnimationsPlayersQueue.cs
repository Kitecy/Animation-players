using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public class AnimationsPlayersQueue : BasePlayer
    {
        [SerializeField] private List<BasePlayer> _players;

        private int _currentPlayer = 0;

        public override void Play(Action onCompleteCallback = null)
        {
            Prepare();
            _players.First().Play(() => PlayNext(onCompleteCallback));
        }

        public override async UniTask AsyncPlay()
        {
            if (OnDisableToken.IsCancellationRequested)
            {
                Stop();
                return;
            }

            Prepare();

            foreach (BasePlayer player in _players)
                await player.AsyncPlay();
        }

        private void PlayNext(Action onCompleteCallback)
        {
            _currentPlayer++;

            if (_currentPlayer < _players.Count)
            {
                _players[_currentPlayer].Play(() => PlayNext(onCompleteCallback));
                return;
            }

            onCompleteCallback?.Invoke();
            _currentPlayer = 0;
        }

        public override void Prepare()
        {
            foreach (BasePlayer player in _players)
            {
                player.Prepare();
            }
        }

        public override void Stop()
        {
            foreach (BasePlayer player in _players)
                player.Stop();
        }
    }
}
