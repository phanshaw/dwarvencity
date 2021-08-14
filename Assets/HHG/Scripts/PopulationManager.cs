using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace HHG.Scripts
{
    [Serializable]
    public struct UIPopulationMapping
    {
        public UnitDefinition targetType;
        public Image objectIcon;
        public Text textObjectCounter;
    }

    [Serializable]
    public class PopulationManager : MonoBehaviour
    {
        // Singleton management. Can I abstract this?
        private static PopulationManager _instance;

        public static PopulationManager Get
        {
            get { return _instance; }
        }

        private bool CreateStaticInstance()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return false;
            }

            _instance = this;
            return true;
        }

        [SerializeField] 
        private ResourceType HappinessResourceType;
        
        [SerializeField]
        private Image HappinessProgressBar;
        private float _currentHappiness;

        [FormerlySerializedAs("_villagerPrefab")] [SerializeField] 
        private UnitDefinition _defaultUnitType;

        private Transform _unitsRoot;
        
        private UnitDefinition[] _globalGathererDefinitions;
        
        [SerializeField] 
        private List<UIPopulationMapping> uiPopulationMapping;
        private Dictionary<UnitDefinition, int> _gathererPopulation;
        public Dictionary<UnitDefinition, int> GathererPopulation
        {
            get
            {
                if (_gathererPopulation == null)
                {
                    _gathererPopulation = new Dictionary<UnitDefinition, int>();
                }

                return _gathererPopulation;
            }
        }

        // TODO: Extract? Keep it here?
        private Dictionary<UnitDefinition, Stack<GameObject>> _populationPool;
        
        private void LoadData()
        {
            _globalGathererDefinitions = Resources.LoadAll<UnitDefinition>("Data");
        }

        public int TotalPopulation
        {
            get
            {
                var total = 0;
                foreach (var populationType in _gathererPopulation)
                {
                    total += populationType.Value;
                }

                return total;
            }
        }

        public void Awake()
        {
            if (!CreateStaticInstance())
                return;

            LoadData();

            _unitsRoot = new GameObject("Units").transform;
            
            _currentHappiness = 0;
            _gathererPopulation = new Dictionary<UnitDefinition, int>();
            
            // Use this to push disabled spawns onto...
            _populationPool = new Dictionary<UnitDefinition, Stack<GameObject>>();

            // Initialize the trackers...
            foreach (var gathererDefinition in _globalGathererDefinitions)
            {
                _gathererPopulation.Add(gathererDefinition, 0);
                _populationPool.Add(gathererDefinition, new Stack<GameObject>());
            }
        }
        
        private void RegisterUnit(UnitDefinition type, GameObject instance)
        {
            if (!_gathererPopulation.ContainsKey(type))
            {
                _gathererPopulation.Add(type, 1);
                return;
            }

            _gathererPopulation[type] += 1;
        }

        private void DeregisterGatherer(UnitDefinition type, GameObject instance)
        {
            Debug.Assert(_gathererPopulation.ContainsKey(type), $"Trying to remove unregistered gatherer type: {type.Name}");
            _gathererPopulation[type] -= 1;
            Debug.Assert(_gathererPopulation[type] >= 0, $"Removing more gatherers than previously added: {type.Name}");
        }

        private void Update()
        {
            if (TotalPopulation < GameManager.Get.GameScenario.BasePopulationCap)
            {
                _currentHappiness += (1f * Time.deltaTime);
                if (_currentHappiness > HappinessResourceType.Max)
                {
                    _currentHappiness = 0;
                    SpawnUnit(_defaultUnitType, Vector3.zero, Quaternion.identity);
                }
            }

            UpdateUI();
        }

        public void ConvertUnitType(GameObject unitGameObject, UnitDefinition sourceUnitType, UnitDefinition targetType)
        {
            // Get the original unit component
            var unitComponent = unitGameObject.GetComponent<UnitComponent>();
            
            // Copy the stats. Maybe eventually we want to keep the names consistent etc for flavor?
            // Maybe these live in a database rather than granular structs? 
            var existingStats = unitComponent.UnitStats;
            
            var t = unitGameObject.transform;
            var location = t.position;
            var rotation = t.rotation;
            
            PopulationManager.Get.DespawnUnit(sourceUnitType, unitGameObject);
            var newUnit = PopulationManager.Get.SpawnUnit(targetType, location, rotation);

            var newUnitComponent = unitGameObject.GetComponent<UnitComponent>();
            newUnitComponent.UnitStats.TransferFrom(existingStats);
        }

        public void DespawnUnit(UnitDefinition type, GameObject unit)
        {
            SelectionManager.Get.RemoveFromSelection(unit.GetComponent<InteractionComponent>());
            DeregisterGatherer(type, unit);
            
            unit.gameObject.SetActive(false);
            _populationPool[type].Push(unit);
        }

        public GameObject SpawnUnit(UnitDefinition unitType, Vector3 position, Quaternion rotation) 
        {
            if(TotalPopulation >= GameManager.Get.GameScenario.BasePopulationCap)
                return null;

            if (_populationPool[unitType].Count > 0)
            {
                var instance = _populationPool[unitType].Pop();
                instance.transform.SetPositionAndRotation(position, rotation);
                RegisterUnit(unitType, instance);
                instance.SetActive(true);
                return instance;
            }
            else
            {
                var instance = PrefabUtility.InstantiatePrefab(unitType.prefabGameObject, _unitsRoot) as GameObject;
                Debug.Assert(instance != null, "PopulationManager: Spawned instance was null!");
                
                instance.transform.SetPositionAndRotation(position, rotation);
                RegisterUnit(unitType, instance);
                return instance;
            }
        }

        private void UpdateUI()
        {
            // Update villager spawning rate 
            var rectTransformLocalScale = HappinessProgressBar.rectTransform.localScale;
            rectTransformLocalScale.x = _currentHappiness / Mathf.Max(HappinessResourceType.Max, 1);
            HappinessProgressBar.rectTransform.localScale = rectTransformLocalScale;
            
            foreach (var populationMapping in uiPopulationMapping)
            {
                populationMapping.objectIcon.sprite = populationMapping.targetType.Icon;
                populationMapping.textObjectCounter.text = _gathererPopulation[populationMapping.targetType].ToString();
            }
        }
    }
}
