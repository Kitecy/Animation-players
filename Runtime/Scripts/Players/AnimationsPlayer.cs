using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public class AnimationsPlayer : BasePlayer
    {
        [SerializeReference] private List<Animation> _animations = new();

        public IReadOnlyList<IReadOnlyAnimation> Animations => _animations.AsReadOnly();

        private List<Sequence> _currentSequences = new();

        private int MaxOrder => _animations.Max(x => x.Order);
        private int MinOrder => _animations.Min(x => x.Order);

        public override void Play(Action onCompleteCallback = null)
        {
            _currentSequences = PrepareForPlay(GetOnDisableCancellationToken());

            if (_currentSequences.Count == 0)
                return;

            Sequence lastSequence = _currentSequences.Last();

            lastSequence.OnComplete(() =>
            {
                _currentSequences.Clear();
                onCompleteCallback?.Invoke();
            });

            ProcessSequences(_currentSequences);

            _currentSequences.First().Play();
        }

        public void Play(string name, Action onCompleteCallback = null)
        {
            Animation animation = GetPreparedAnimation(name);

            if (animation == null)
                return;

            Tween tween = animation.Convert(this, IsUI, GetOnDisableCancellationToken());
            tween.Play().OnComplete(() => onCompleteCallback.Invoke());
        }

        public override async UniTask AsyncPlay(CancellationToken token)
        {
            _currentSequences = PrepareForPlay(token);

            if (_currentSequences.Count == 0)
                return;

            Sequence lastSequence = _currentSequences.Last();

            ProcessSequences(_currentSequences);

            _ = _currentSequences.First().Play();

            await lastSequence.AsyncWaitForCompletion();
        }

        public async UniTask AsyncPlay(string name, CancellationToken token)
        {
            Animation animation = GetPreparedAnimation(name);

            if (animation == null)
                return;

            CancellationTokenSource source = CombineTokensWithOnDisableToken(token);

            Tween tween = animation.Convert(this, IsUI, source.Token);
            await tween.Play();
        }

        public override void Prepare()
        {
            List<Animation> animations = _animations.FindAll(x => x.Order == MinOrder);

            foreach (Animation animation in animations)
                animation.Prepare(this, IsUI);
        }

        private Animation GetPreparedAnimation(string name)
        {
            Animation animation = _animations.FirstOrDefault(anim => anim.Name == name);

            if (animation == null)
            {
                Debug.LogError("An animation with this name was not found.");
                return null;
            }

            animation.Prepare(this, IsUI);
            return animation;
        }

        private List<Sequence> CreateSequences(CancellationToken token)
        {
            List<Sequence> sequences = new List<Sequence>();

            CancellationTokenSource source = CombineTokensWithOnDisableToken(token);

            for (int order = MinOrder; order <= MaxOrder; order++)
            {
                List<Animation> animationsInOrder = _animations.FindAll(x => x.Order == order);

                if (animationsInOrder.Count > 0)
                {
                    Sequence sequence = DOTween.Sequence().Pause();

                    foreach (Animation animation in animationsInOrder)
                    {
                        Tween tween = animation.Convert(this, IsUI, source.Token);
                        sequence.Join(tween);
                    }

                    sequence.WithCancellation(source.Token);

                    sequences.Add(sequence);
                }
            }

            return sequences;
        }

        private List<Sequence> PrepareForPlay(CancellationToken token)
        {
            Prepare();
            List<Sequence> sequences = CreateSequences(token);

            return sequences;
        }

        private void ProcessSequences(List<Sequence> sequences)
        {
            if (sequences.Count > 1)
            {
                for (int i = 0; i < sequences.Count - 1; i++)
                {
                    sequences[i].OnComplete(() =>
                    {
                        int nextId = i++;

                        if (sequences.Count > nextId)
                            sequences[nextId]?.Play();
                    });
                }
            }
        }
    }
}