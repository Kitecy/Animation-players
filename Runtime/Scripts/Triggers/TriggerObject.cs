using System;
using UnityEngine;

namespace AnimationPlayers.Players
{
    [Serializable]
    public class TriggerObject
    {
        [SerializeField] private BaseTrigger _trigger;
        [SerializeField] private Animation _animation = new();

        public TriggerObject(BaseTrigger trigger, Animation animation)
        {
            _trigger = trigger;
            _animation = animation;
        }

        public BaseTrigger Trigger => _trigger;
        public IReadOnlyAnimation Animation => _animation;
    }
}
