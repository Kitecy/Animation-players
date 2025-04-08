namespace AnimationPlayers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UnityEngine;

    public sealed class AnimationQueuePlayer : MonoBehaviour
    {
        [SerializeField, SerializeInterface(typeof(IPlayer))] private List<GameObject> _animationPlayers;
        [SerializeField] private bool _playOnEnable;

        private Action _onAnimationEnded;

        private void OnEnable()
        {
            if (_playOnEnable)
                StartCoroutine(PlayWhenOnEnable());
        }

        public async void Play(Action onAnimationEnded = null)
        {
            if (onAnimationEnded != null)
                _onAnimationEnded = onAnimationEnded;

            List<IPlayer> players = _animationPlayers.Select(p => p.GetComponent<IPlayer>()).ToList();

            foreach (IPlayer player in players)
            {
                if (player != null)
                    await AsyncProcessPlayer(player);
            }
        }

        private async Task AsyncProcessPlayer(IPlayer player)
        {
            switch (player)
            {
                case AnimationPlayer animationPlayer:
                    await animationPlayer.AsyncPlayAll();
                    break;

                case SimpleAnimationPlayer simpleAnimationPlayer:
                    await simpleAnimationPlayer.AsyncPlay();
                    break;
            }
        }

        private IEnumerator PlayWhenOnEnable()
        {
            List<IPlayer> players = _animationPlayers.Select(p => p.GetComponent<IPlayer>()).ToList();

            foreach (IPlayer player in players)
            {
                MonoBehaviour behaviour = player as MonoBehaviour;

                while (behaviour.didAwake == false)
                    yield return null;

                Task task = AsyncProcessPlayer(player);

                while (task.IsCompleted == false)
                    yield return null;
            }

            //foreach (AnimationPlayer player in _animationPlayers)
            //{
            //    while (player.didAwake == false)
            //        yield return null;

            //    Task task = player.AsyncPlayAll();

            //    while (task.IsCompleted == false)
            //        yield return null;
            //}

            _onAnimationEnded?.Invoke();
            _onAnimationEnded = null;
        }
    }
}