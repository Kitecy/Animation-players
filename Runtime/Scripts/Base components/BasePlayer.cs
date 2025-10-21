using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public abstract class BasePlayer : MonoBehaviour, IAnimationPlayer
    {
        [SerializeField] protected bool IsUI;

        [SerializeField] private AutoCall _autoCall;

        private GroupedAnimationPlayers _parentPlayer;
        protected CancellationTokenSource _onDisableCancellationTokenSource;

        public enum AutoCall
        {
            None,
            Awake,
            OnEnable
        }

        protected AutoCall Call => _autoCall;

        protected CancellationToken OnDisableToken => _onDisableCancellationTokenSource != null ? _onDisableCancellationTokenSource.Token : new CancellationToken(true);
        public bool IsUsingInUI => IsUI;

        private void Awake()
        {
            _parentPlayer = GetComponentInParent<GroupedAnimationPlayers>();

            if (_parentPlayer == this)
                _parentPlayer = null;

            if (_parentPlayer != null)
                return;

            if (_autoCall == AutoCall.Awake)
                Play();
        }

        private void OnEnable()
        {
            _onDisableCancellationTokenSource = new CancellationTokenSource();

            if (_autoCall != AutoCall.None)
                Prepare();

            if (_autoCall == AutoCall.OnEnable && _parentPlayer == null)
                Play();
        }

        private void OnDisable()
        {
            _onDisableCancellationTokenSource.Cancel();
            _onDisableCancellationTokenSource.Dispose();
            _onDisableCancellationTokenSource = null;

            Stop();
        }

        public abstract void Play(Action onCompleteCallback = null);

        public abstract UniTask AsyncPlay();

        public abstract void Prepare();

        public abstract void Stop();
    }
}
