using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace HHG.Scripts
{
    [Serializable]
    public struct ResourceQuantityMap
    {
        public ResourceType Type;
        public int Amount;
    }

    [Serializable]
    public struct PopulationQuantityMap
    {
        public UnitDefinition Type;
        public int Amount;
    }

    [CreateAssetMenu(fileName = "GameScenario", menuName = "HHG/Data/New Game Scenario Definition", order = 1)]
    public class GameScenarioDefinition : ScriptableObject
    {
        [SerializeField] 
        private string _scenarioName;
        public string ScenarioName => _scenarioName;

        [SerializeField] private int basePopulationCap;
        public int BasePopulationCap => basePopulationCap;
        
        [SerializeField]
        private List<PopulationQuantityMap> _startingUnitTypes;
        public IEnumerable<PopulationQuantityMap> StartingUnitTypes => _startingUnitTypes;
        
        [SerializeField]
        private List<ResourceQuantityMap> _startingResources;
        public IEnumerable<ResourceQuantityMap> StartingResources => _startingResources;
    }
}
