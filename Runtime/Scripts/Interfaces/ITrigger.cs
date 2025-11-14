using System;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public interface ITrigger
    {
        event Action<ITrigger> Triggered;

        void Invoke();
    }
}
