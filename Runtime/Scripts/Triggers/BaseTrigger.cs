using System;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public class BaseTrigger : MonoBehaviour, ITrigger
    {
        public event Action<ITrigger> Triggered;

        public virtual void Invoke()
        {
            InternalInvoke();
        }

        protected void InternalInvoke()
        {
            Triggered?.Invoke(this);
        }
    }
}
