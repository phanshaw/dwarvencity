using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HHG.Scripts
{
    [Serializable]
    public enum EResourceBehavior
    {
        Accumulate,
        CyclicalAccumulation
    }

    [CreateAssetMenu(fileName = "ResourceType", menuName = "HHG/Data/New Resource Definition", order = 1)]
    public class ResourceType : ScriptableObject
    {
        public string Name;
        public Sprite Icon;
        public EResourceBehavior Behavior;
        public bool CanGoNegative = false;
        public int Max = 800;
    }
}
