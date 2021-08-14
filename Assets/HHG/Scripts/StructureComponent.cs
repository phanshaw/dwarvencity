using UnityEngine;

namespace HHG.Scripts
{
    public class StructureComponent : MonoBehaviour
    {
        [SerializeField] 
        private StructureDefinition _structureType;
        public StructureDefinition StructureType => _structureType;
    }
}
