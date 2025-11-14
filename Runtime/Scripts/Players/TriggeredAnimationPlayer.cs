using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public class TriggeredAnimationPlayer : BasePlayer
    {
        private const string SameObjectContainsInTriggersError = "Such a trigger has already been added!";
        private const string PlayError = "You can run this method only after its first call using Trigger!";

        [SerializeReference] private List<TriggerObject> _triggers = new();

        private CancellationTokenSource _playRestartedSource;

        private IReadOnlyAnimation _current;

        public void AddTrigger(BaseTrigger trigger, Animation animation)
        {
            if (trigger == null)
                throw new ArgumentNullException(nameof(trigger));

            if (animation == null)
                throw new ArgumentNullException(nameof(animation));

            TriggerObject foundTrigger = _triggers.FirstOrDefault(x => x.Trigger == trigger);

            if (foundTrigger != null)
            {
                Debug.LogError(SameObjectContainsInTriggersError);
                return;
            }

            _triggers.Add(new TriggerObject(trigger, animation));

            trigger.Triggered += OnTriggered;
        }

        public override async UniTask AsyncPlay(CancellationToken token)
        {
            if (_current == null)
            {
                Debug.LogError(PlayError);
                return;
            }

            _playRestartedSource = CombineTokensWithOnDisableToken(token);

            Prepare();
            Tween tween = _current.Convert(this, IsUI, _playRestartedSource.Token);

            await tween.Play();

            _playRestartedSource = null;
        }

        public override void Play(Action onCompleteCallback = null)
        {
            if (_current == null)
            {
                Debug.LogError(PlayError);
                return;
            }

            Prepare();

            _playRestartedSource = CombineTokensWithOnDisableToken();

            Tween tween = _current.Convert(this, IsUI, _playRestartedSource.Token);
            tween.Play().OnComplete(() =>
            {
                onCompleteCallback?.Invoke();
                _playRestartedSource = null;
            });
        }

        public override void Prepare()
        {
            if (_current == null)
            {
                Debug.LogError(PlayError);
                return;
            }

            _current.Prepare(this, IsUI);
        }

        protected override void OnEnabled()
        {
            foreach (TriggerObject trigger in _triggers)
            {
                trigger.Trigger.Triggered += OnTriggered;
            }
        }

        protected override void OnDisabled()
        {
            foreach (TriggerObject trigger in _triggers)
            {
                trigger.Trigger.Triggered -= OnTriggered;
            }
        }

        private void OnTriggered(ITrigger trigger)
        {
            if (_playRestartedSource != null)
            {
                _playRestartedSource.Cancel();
                _playRestartedSource.Dispose();
                _playRestartedSource = null;
            }

            _current = _triggers.FirstOrDefault(x => (ITrigger)x.Trigger == trigger).Animation;
            Play();
        }
    }
}
