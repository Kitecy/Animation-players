using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public class AnimationsPlayer : BasePlayer
    {
        [SerializeField] private List<Animation> _animations = new();

        public IReadOnlyList<IReadOnlyAnimation> Animations => _animations.AsReadOnly();

        private int MaxOrder => _animations.Max(x => x.Order);
        private int MinOrder => _animations.Min(x => x.Order);

        public override void Play(Action onCompleteCallback = null)
        {
            List<Sequence> sequences = PrepareForPlay();

            if (sequences.Count <= 0)
                return;

            Sequence lastSequence = sequences.Last();
            lastSequence.OnComplete(() => onCompleteCallback?.Invoke());

            ProcessSequences(sequences);

            sequences.First().Play();
        }

        public override async UniTask AsyncPlay()
        {
            List<Sequence> sequences = PrepareForPlay();

            if (sequences.Count <= 0)
                return;

            Sequence lastSequence = sequences.Last();

            ProcessSequences(sequences);

            sequences.First().Play();

            await lastSequence.AsyncWaitForCompletion();
        }

        public override void Prepare()
        {
            List<Animation> animations = _animations.FindAll(x => x.Order == MinOrder);

            foreach (Animation animation in animations)
                animation.Prepare(this, IsUI);
        }

        private List<Sequence> CreateSequences()
        {
            List<Sequence> sequences = new List<Sequence>();

            for (int order = MinOrder; order <= MaxOrder; order++)
            {
                List<Animation> animationsInOrder = _animations.FindAll(x => x.Order == order);

                if (animationsInOrder.Count > 0)
                {
                    Sequence sequence = DOTween.Sequence().Pause();

                    foreach (Animation animation in animationsInOrder)
                    {
                        Tween tween = animation.Convert(this, IsUI);
                        sequence.Join(tween);
                    }

                    sequences.Add(sequence);
                }
            }

            return sequences;
        }

        private List<Sequence> PrepareForPlay()
        {
            Prepare();
            List<Sequence> sequences = CreateSequences();

            return sequences;
        }

        private void ProcessSequences(List<Sequence> sequences)
        {
            if (sequences.Count > 1)
            {
                for (int i = 0, x = 1; i < sequences.Count - 1; i++, x++)
                {
                    sequences[i].OnComplete(() => sequences[x]?.Play());
                }
            }
        }
    }
}