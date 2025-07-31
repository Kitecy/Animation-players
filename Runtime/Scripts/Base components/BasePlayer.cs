using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public abstract class BasePlayer : MonoBehaviour, IAnimationPlayer
    {
        [SerializeField] protected bool IsUI;

        [SerializeField] private AutoCall _autoCall;

        private GroupedAnimationPlayers _parentPlayer;

        public enum AutoCall
        {
            None,
            Awake,
            OnEnable
        }

        public bool IsUsingInUI => IsUI;

        private void Awake()
        {
            _parentPlayer = GetComponentInParent<GroupedAnimationPlayers>();

            if (_parentPlayer != null)
                return;

            if (_autoCall == AutoCall.Awake)
                Play();
        }

        private void OnEnable()
        {
            if (_parentPlayer != null)
                return;

            if (_autoCall == AutoCall.OnEnable)
                Play();
        }

        private void OnDisable()
        {
            this.DOKill();
        }

        public abstract void Play(Action onCompleteCallback = null);

        public abstract UniTask AsyncPlay();

        public abstract void Prepare();
    }
}
