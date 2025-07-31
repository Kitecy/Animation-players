using Cysharp.Threading.Tasks;
using System;

namespace AnimationPlayers.Players
{
    public interface IAnimationPlayer
    {
        void Play(Action onCompleteCallback);

        UniTask AsyncPlay();

        void Prepare();
    }
}
