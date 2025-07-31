using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace AnimationPlayers.Players
{
    public interface IReadOnlyAnimation
    {
        public string Name { get; }
        public int Order { get; }

        public float Duration { get; }
        public float Delay { get; }

        public bool IsEternalLoop { get; }
        public int Loops { get; }
        public LoopType LoopType { get; }

        public Ease Ease { get; }
        public Animation.Type Sort { get; }

        public Vector3 StartPosition { get; }
        public Vector3 EndPosition { get; }

        public Vector3 StartRotation { get; }
        public Vector3 EndRotation { get; }

        public Vector3 StartScale { get; }
        public Vector3 EndScale { get; }

        public Color StartColor { get; }
        public Color EndColor { get; }

        public Renderer Renderer { get; }
        public Graphic Graphic { get; }

        public float StartFade { get; }
        public float EndFade { get; }

        public float TotalDuration { get; }
    }
}
