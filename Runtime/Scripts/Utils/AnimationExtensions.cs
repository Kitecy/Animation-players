using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public static class AnimationExtensions
    {
        private const string ConvertError = "The type you are trying to convert is not supported!";
        private const string PrepareError = "The type you are trying to prepare is not supported!";
        private const string UIError = "You can't use an Anchor-type animation not with a UI object!";

        public static void Prepare(this IReadOnlyAnimation animation, BasePlayer player, bool isUI)
        {
            switch (animation.Sort)
            {
                case Animation.Type.Position:
                    player.transform.position = animation.StartPosition;
                    break;

                case Animation.Type.Rotation:
                    player.transform.rotation = Quaternion.Euler(animation.StartRotation);
                    break;

                case Animation.Type.Scale:
                    player.transform.localScale = animation.StartScale;
                    break;

                case Animation.Type.Color:
                    if (isUI)
                        animation.Graphic.color = animation.StartColor;
                    else
                        animation.Renderer.material.color = animation.StartColor;
                    break;

                case Animation.Type.Fade:
                    if (isUI)
                    {
                        Color color = animation.Graphic.color;

                        animation.Graphic.color = new Color(color.r, color.g, color.b, animation.StartFade);
                    }
                    else
                    {
                        Color color = animation.Renderer.material.color;

                        animation.Renderer.material.color = new Color(color.r, color.g, color.b, animation.StartFade);
                    }
                    break;

                case Animation.Type.Anchor:
                    if (isUI == false)
                        throw new System.InvalidProgramException(UIError);

                    (player.transform as RectTransform).anchoredPosition = animation.StartAnchorPosition;
                    break;

                default:
                    throw new System.InvalidOperationException(PrepareError);
            }
        }

        public static Tween Convert(this IReadOnlyAnimation animation, BasePlayer player, bool isUI, CancellationToken token)
        {
            if (animation == null)
                throw new System.ArgumentNullException(nameof(animation));

            if (player == null)
                throw new System.ArgumentNullException(nameof(player));

            Tween tween;

            switch (animation.Sort)
            {
                case Animation.Type.Position:
                    tween = player.transform.DOMove(animation.EndPosition, animation.Duration).Pause();
                    break;

                case Animation.Type.Rotation:
                    tween = player.transform.DORotate(animation.EndRotation, animation.Duration).Pause();
                    break;

                case Animation.Type.Scale:
                    tween = player.transform.DOScale(animation.EndScale, animation.Duration).Pause();
                    break;

                case Animation.Type.Color:
                    if (isUI)
                        tween = animation.Graphic.DOColor(animation.EndColor, animation.Duration).Pause();
                    else
                        tween = animation.Renderer.material.DOColor(animation.EndColor, animation.Duration).Pause();
                    break;

                case Animation.Type.Fade:
                    if (isUI)
                        tween = animation.Graphic.DOColor(animation.EndColor, animation.Duration).Pause();
                    else
                        tween = animation.Renderer.material.DOColor(animation.EndColor, animation.Duration).Pause();
                    break;

                case Animation.Type.Anchor:
                    if (isUI == false)
                        throw new System.InvalidOperationException(UIError);

                    tween = (player.transform as RectTransform).DOAnchorPos(animation.EndAnchorPosition, animation.Duration).Pause();
                    break;

                default:
                    throw new System.InvalidOperationException(ConvertError);
            }

            tween.SetDelay(animation.Delay).SetEase(animation.Ease).SetLoops(animation.Loops, animation.LoopType);
            tween.WithCancellation(token);

            return tween;
        }
    }
}
