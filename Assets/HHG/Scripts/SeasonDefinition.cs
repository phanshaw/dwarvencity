using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace HHG.Scripts
{
    [CreateAssetMenu(fileName = "SeasonType", menuName = "HHG/Data/New Season Definition", order = 1)]
    public class SeasonDefinition : ScriptableObject
    {
        [SerializeField] 
        private Sprite _icon;
        public Sprite icon => _icon;
        
        [SerializeField] 
        private string _seasonName;
        public string seasonName => _seasonName;

        [SerializeField] 
        private string _description;
        public string Description => _description;

        [SerializeField] 
        private float _durationSeconds = 20f;
        public float DurationSeconds => _durationSeconds;
        
        [SerializeField] 
        private List<ResourceModifier> _globalResourceModifiers;
        public List<ResourceModifier> globalResourceModifiers => _globalResourceModifiers;
        
        // Add any additional system modifiers here...
    }
}
