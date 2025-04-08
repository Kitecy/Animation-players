namespace AnimationPlayers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using UnityEngine;

    public sealed class AnimationQueuePlayer : MonoBehaviour
    {
        [SerializeField] private List<AnimationPlayer> _animationPlayers;
        [SerializeField] private bool _playOnEnable;

        private Action _onAnimationEnded;

        private void OnEnable()
        {
            if (_playOnEnable)
                StartCoroutine(PlayWhenOnEnable());
        }

        public async void Play(Action onAnimationEnded)
        {
            if (onAnimationEnded != null)
                _onAnimationEnded = onAnimationEnded;

            foreach (AnimationPlayer player in _animationPlayers)
            {
                await player.AsyncPlayAll();
            }
        }

        private IEnumerator PlayWhenOnEnable()
        {
            foreach (AnimationPlayer player in _animationPlayers)
            {
                while (player.didAwake == false)
                    yield return null;

                Task task = player.AsyncPlayAll();

                while (task.IsCompleted == false)
                    yield return null;
            }

            _onAnimationEnded?.Invoke();
            _onAnimationEnded = null;
        }
    }
}