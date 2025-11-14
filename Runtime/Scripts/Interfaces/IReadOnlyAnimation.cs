using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace AnimationPlayers.Players
{
    public interface IReadOnlyAnimation
    {
        string Name { get; }
        int Order { get; }

        float Duration { get; }
        float Delay { get; }

        bool IsEternalLoop { get; }
        int Loops { get; }
        LoopType LoopType { get; }

        Ease Ease { get; }
        Animation.Type Sort { get; }

        Vector3 StartPosition { get; }
        Vector3 EndPosition { get; }

        Vector3 StartRotation { get; }
        Vector3 EndRotation { get; }

        Vector3 StartScale { get; }
        Vector3 EndScale { get; }

        Color StartColor { get; }
        Color EndColor { get; }

        Renderer Renderer { get; }
        Graphic Graphic { get; }

        float StartFade { get; }
        float EndFade { get; }

        Vector2 StartAnchorPosition { get; }
        Vector2 EndAnchorPosition { get; }

        float TotalDuration { get; }
    }
}
