namespace AnimationPlayers
{
    using System;
    using UnityEngine;

    public class SerializeInterfaceAttribute : PropertyAttribute
    {
        public SerializeInterfaceAttribute(Type type)
        {
            Type = type;
        }

        public Type Type { get; private set; }
    }
}
