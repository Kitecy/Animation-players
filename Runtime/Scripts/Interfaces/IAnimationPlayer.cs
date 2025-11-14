using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace AnimationPlayers.Players
{
    public interface IAnimationPlayer
    {
        void Play(Action onCompleteCallback);

        UniTask AsyncPlay(CancellationToken token);

        void Prepare();
    }
}
