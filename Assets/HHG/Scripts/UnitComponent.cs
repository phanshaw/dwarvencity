using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace HHG.Scripts
{
    public class UnitComponent : MonoBehaviour
    {
        [SerializeField]
        private UnitDefinition unitType;
        public UnitDefinition UnitType => unitType;

        [SerializeField]
        private ETeam team = ETeam.Team1;

        public ETeam Team => team;

        private UnitStats _unitStats;
        public UnitStats UnitStats => _unitStats;

        public void Awake()
        {
            _unitStats.Init(unitType);
        }

        // Logic
        public void TakeDamage(int amount)
        {
            _unitStats.TakeDamage(amount);
            if(!_unitStats.IsAlive)
                Die();
        }

        private void Die()
        {
            Debug.Log($"This unit {gameObject.name} died.");
            
            // TODO: Register this with the population manager to trigger a death state...
        }

        public bool NeedsHealing()
        {
            return _unitStats.NeedsHealing();
        }

        public void Heal(int amount)
        {
            if(!_unitStats.NeedsHealing())
                return;

            _unitStats.Heal(amount);
        }
    }
}
