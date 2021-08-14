using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace HHG.Scripts
{
    [Serializable]
    public enum EResourceAffectType
    {
        Gain, 
        Overhead,
        MultiGlobal,
        OverheadPerPop
    }

    [Serializable]
    public class ResourceModifier
    {
        [SerializeField] 
        private string _description;
        public string Description => _description;
        
        [SerializeField] 
        private ResourceType _resource;
        public ResourceType Resource => _resource;
        
        [SerializeField] 
        private EResourceAffectType _modifierType;

        public EResourceAffectType ModifierType => _modifierType;
        
        [SerializeField] 
        private float _amount;

        public float Amount
        {
            get
            {
                switch (_modifierType)
                {
                    case EResourceAffectType.Gain:
                        return _amount;
                    case EResourceAffectType.Overhead:
                        return -_amount;
                    case EResourceAffectType.MultiGlobal:
                        return _amount;
                    case EResourceAffectType.OverheadPerPop:
                        return -_amount;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
