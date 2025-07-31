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

        private bool _isFirstOnEnable = true;

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

        private void OnEnable()
        {
            if (_isFirstOnEnable == false && _playOnEnable)
                Play();

            _isFirstOnEnable = false;
        }

        public override void Play(Action onCompleteCallback = null)
        {
            Prepare();

            _intervalWait = new WaitForSeconds(_interval);

            if (_player != null)
            {
                _player.Play(() => StartCoroutine(ProcessNext(onCompleteCallback)));
                return;
            }

            _players.First().Play(() => StartCoroutine(ProcessNext(onCompleteCallback)));
        }

        public override async UniTask AsyncPlay()
        {
            Prepare();

            TimeSpan delay = TimeSpan.FromSeconds(_interval);

            if (_player != null)
            {
                await _player.AsyncPlay();
                await UniTask.Delay(delay);
            }

            for (int i = 0; i < _players.Count; i++)
            {
                await _players[i].AsyncPlay();

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

        private IEnumerator ProcessNext(Action onCompleteCallback = null)
        {
            yield return _intervalWait;

            _currentPlayer++;

            if (_currentPlayer < _players.Count)
            {
                _players[_currentPlayer].Play(() => StartCoroutine(ProcessNext(onCompleteCallback)));
                yield break;
            }

            onCompleteCallback?.Invoke();
            _currentPlayer = 0;
        }

        public override void Prepare()
        {
            _player.Prepare();

            foreach (BasePlayer player in _players)
                player.Prepare();
        }
    }
}