namespace AnimationPlayers
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using UnityEngine;

    [RequireComponent(typeof(IPlayer))]
    public class AnimationPlayersGroup : MonoBehaviour
    {
        [SerializeField, SerializeInterface(typeof(IPlayer))] private List<GameObject> _playersObjects;
        [SerializeField] private bool _playOnEnable;
        [SerializeField] private float _interval;

        private IPlayer _player;
        private List<IPlayer> _players;

        private WaitForSeconds _intervalWait;

        private void Awake()
        {
            _players = _playersObjects.Select(p => p.GetComponent<IPlayer>()).ToList();

            _intervalWait = new WaitForSeconds(_interval);
        }

        private void OnEnable()
        {
            _player = GetComponent<IPlayer>();

            if (_playOnEnable)
                StartCoroutine(PlayOnEnable());
        }

        public async void Play()
        {
            await AsyncProcessPlayer(_player);

            foreach (IPlayer player in _players)
            {
                _ = AsyncProcessPlayer(player);
            }
        }

        private IEnumerator PlayOnEnable()
        {
            yield return WaitForAwake();

            SetStartsValues();

            Task mainPlayerTask = AsyncProcessPlayer(_player);

            while (mainPlayerTask.IsCompleted == false)
                yield return null;

            foreach (IPlayer player in _players)
            {
                _ = AsyncProcessPlayer(player);

                yield return _intervalWait;
            }
        }

        private void SetStartsValues()
        {
            foreach (IPlayer player in _players)
                player.SetStartValue();
        }

        private IEnumerator WaitForAwake()
        {
            MonoBehaviour mainPlayerBehaviour = _player as MonoBehaviour;

            while (mainPlayerBehaviour.didAwake == false)
                yield return null;

            foreach (IPlayer player in _players)
            {
                MonoBehaviour behaviour = player as MonoBehaviour;

                while (behaviour.didAwake == false)
                    yield return null;
            }
        }

        private async Task AsyncProcessPlayer(IPlayer player)
        {
            switch (player)
            {
                case SimpleAnimationPlayer simpleAnimationPlayer:
                    await simpleAnimationPlayer.AsyncPlay();
                    break;

                case AnimationPlayer animationPlayer:
                    await animationPlayer.AsyncPlayAll();
                    break;
            }
        }

        [ContextMenu("Set players")]
        private void SetAllPlayersFromChildren()
        {
            _playersObjects = GetComponentsInChildren<MonoBehaviour>().Where(p => p is IPlayer).Distinct().Select(p => p.gameObject).ToList();
            _playersObjects.Remove(gameObject);
        }
    }
}