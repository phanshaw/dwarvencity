using System.Collections.Generic;
using UnityEngine;

namespace HHG.Scripts
{
    public enum ETeam
    {
        Team1 = 0,
        Team2,
        Team3,
        Team4
    }

    [CreateAssetMenu(fileName = "GathererType", menuName = "HHG/Data/New Gatherer Definition", order = 1)]
    public class UnitDefinition : ScriptableObject
    {
        [SerializeField] 
        private string _name;
        public string Name => _name;
        
        [SerializeField] 
        private string _pluralName;
        public string PluralName => _pluralName;
        
        [SerializeField] 
        private string _description;
        public string Description => _description;

        [SerializeField] 
        private Sprite _icon;
        public Sprite Icon => _icon;

        [SerializeField] 
        private GameObject _prefabGameObject;
        public GameObject prefabGameObject => _prefabGameObject;
        
        [SerializeField] 
        private List<ResourceModifier> _resourceModifiers;
        public List<ResourceModifier> ResourceModifiers => _resourceModifiers;

        [SerializeField] 
        private int _maxHealth = 100;
        public int MaxHealth => _maxHealth;
    }
}
