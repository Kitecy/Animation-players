using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public class GroupedAnimationPlayers : BasePlayer
    {
        private readonly string _error = "You cannot specify this component in this field!";

        [SerializeField] private BasePlayer _player;
        [SerializeField] private List<BasePlayer> _players;
        [SerializeField] private float _interval = 0.125f;
        [SerializeField] private bool _playOnEnable;

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

        public override void Play(Action onCompleteCallback = null)
        {
            Prepare();

            _intervalWait = new WaitForSeconds(_interval);

            if (_player != null)
            {
                StartCoroutine(ProcessSelf(onCompleteCallback));
                return;
            }

            StartCoroutine(ProcessNext(onCompleteCallback));
        }

        public override async UniTask AsyncPlay()
        {
            Prepare();

            TimeSpan delay = TimeSpan.FromSeconds(_interval);

            if (_player != null)
            {
                _player.Play();
                await UniTask.Delay(delay);
            }

            for (int i = 0; i < _players.Count; i++)
            {
                _players[i].Play();

                if (i < _players.Count - 1)
                    await UniTask.Delay(delay);
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

        private IEnumerator ProcessSelf(Action onCompleteCallback = null)
        {
            _player.Play();

            yield return _intervalWait;

            if (_currentPlayer < _players.Count)
            {
                StartCoroutine(ProcessNext(onCompleteCallback));
            }

            onCompleteCallback?.Invoke();
            _currentPlayer = 0;
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
            _currentPlayer = 0;
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