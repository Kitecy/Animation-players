using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public abstract class BasePlayer : MonoBehaviour, IAnimationPlayer
    {
        [SerializeField] protected bool IsUI;

        [SerializeField] private AutoCall _autoCall = AutoCall.None;

        private GroupedAnimationPlayers _parentPlayer;
        protected CancellationTokenSource _onDisableCancellationTokenSource;

        public enum AutoCall
        {
            None,
            Awake,
            OnEnable
        }

        protected AutoCall Call => _autoCall;

        public bool IsUsingInUI => IsUI;

        private void Awake()
        {
            _parentPlayer = GetComponentInParent<GroupedAnimationPlayers>();

            if (_parentPlayer == this)
                _parentPlayer = null;

            if (_autoCall == AutoCall.Awake && (_parentPlayer == null || _parentPlayer.enabled == false))
                Play();
        }

        private void OnEnable()
        {
            if (_autoCall != AutoCall.None)
                Prepare();

            if (_autoCall == AutoCall.OnEnable && (_parentPlayer == null || _parentPlayer.enabled == false))
                Play();

            OnEnabled();
        }

        private void OnDisable()
        {
            if (_onDisableCancellationTokenSource != null)
            {
                _onDisableCancellationTokenSource.Cancel();
                _onDisableCancellationTokenSource.Dispose();
                _onDisableCancellationTokenSource = null;
            }

            OnDisabled();
        }

        protected virtual void OnEnabled() { }

        protected virtual void OnDisabled() { }

        protected CancellationToken GetOnDisableCancellationToken()
        {
            if (_onDisableCancellationTokenSource == null)
                _onDisableCancellationTokenSource = new CancellationTokenSource();

            return _onDisableCancellationTokenSource.Token;
        }

        protected CancellationTokenSource CombineTokensWithOnDisableToken(params CancellationToken[] tokens)
        {
            HashSet<CancellationToken> uniqueTokens = new(tokens);

            var disableToken = GetOnDisableCancellationToken();

            if (disableToken.CanBeCanceled)
                uniqueTokens.Add(disableToken);

            return CancellationTokenSource.CreateLinkedTokenSource(uniqueTokens.ToArray());
        }

        public abstract void Play(Action onCompleteCallback = null);

        public abstract UniTask AsyncPlay(CancellationToken token);

        public abstract void Prepare();
    }
}
