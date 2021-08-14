using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace HHG.Scripts
{
    [CreateAssetMenu(fileName = "Structure", menuName = "HHG/Data/New Structure Definition", order = 1)]
    public class StructureDefinition : ScriptableObject
    {
        [SerializeField] 
        private string _structureName;
        public string StructureName => _structureName;

        [FormerlySerializedAs("_resourceModifiers")] [SerializeField] 
        private List<ResourceModifier> _overheadCosts;
        public List<ResourceModifier> OverheadCosts => _overheadCosts;

        [FormerlySerializedAs("interactionBehavior")] [SerializeField] 
        private InteractionBehavior unitInteractionBehavior;
        public InteractionBehavior UnitInteractionBehaviorBehavior => unitInteractionBehavior;
    }
}
