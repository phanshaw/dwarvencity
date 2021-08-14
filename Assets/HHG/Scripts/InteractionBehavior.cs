using UnityEngine;

namespace HHG.Scripts
{
    public abstract class InteractionBehavior : ScriptableObject
    {
        public abstract void ExecuteAction(InteractionComponent instigator);
    }
}
