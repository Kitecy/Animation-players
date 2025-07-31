using System;
using UnityEngine;

namespace AnimationPlayers.Players
{
    public class SerializeInterfaceAttribute : PropertyAttribute
    {
        public SerializeInterfaceAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; private set; }
    }
}
