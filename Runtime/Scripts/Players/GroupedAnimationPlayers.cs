using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public class GroupedAnimationPlayers : BasePlayer
    {
        private readonly string _error = "You cannot specify this component in this field!";

        [SerializeField] private BasePlayer _player;
        [SerializeField] private List<BasePlayer> _players;
        [SerializeField] private float _interval = 0.125f;

        private WaitForSeconds _intervalWait;

        private int _currentPlayer = -1;

        private void OnValidate()
        {
            if (_player == this)
            {
                _player = null;
                Debug.LogError(_error);
            }
        }

        protected override void OnDisableVirtual()
        {
            if (Call != AutoCall.None)
                Prepare();
        }

        public override void Play(Action onCompleteCallback = null)
        {
            PlayInternal(onCompleteCallback).Forget();
        }

        private async UniTaskVoid PlayInternal(Action onCompleteCallback)
        {
            if (_player.didStart == false)
                await UniTask.Yield();

            await AsyncPlay();
            onCompleteCallback?.Invoke();
        }

        public override async UniTask AsyncPlay()
        {
            Prepare();

            TimeSpan delay = TimeSpan.FromSeconds(_interval);

            if (OnDisableToken.IsCancellationRequested)
            {
                Stop();
                return;
            }

            if (_player != null)
            {
                await _player.AsyncPlay();
                await UniTask.Delay(delay, cancellationToken: OnDisableToken);
            }


            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].AsyncPlay().Forget();

                if (i < _players.Count - 1)
                    await UniTask.Delay(delay, cancellationToken: OnDisableToken);
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

        private IEnumerator ProcessNext(Action onCompleteCallback = null)
        {
            _currentPlayer++;

            if (_currentPlayer < _players.Count)
            {
                _players[_currentPlayer].Play();

                yield return _intervalWait;

                StartCoroutine(ProcessNext(onCompleteCallback));

                yield break;
            }

            onCompleteCallback?.Invoke();
            _currentPlayer = -1;
        }

        public override void Prepare()
        {
            if (_player != null)
                _player.Prepare();

            foreach (BasePlayer player in _players)
                player.Prepare();
        }

        public override void Stop()
        {
            if (_player != null)
                _player.Stop();

            foreach (BasePlayer player in _players)
                player.Stop();
        }
    }
}