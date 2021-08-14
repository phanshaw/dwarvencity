using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HHG.Scripts
{
    [Serializable]
    public struct UIResourceMapping
    {
        public ResourceType targetType;
        public Image objectIcon;
        public Text textObjectTotal;
        public Text textObjectDelta;
    }

    public class ResourceManager : MonoBehaviour
    {
        // Singleton management. Can I abstract this?
        private static ResourceManager _instance;
        public static ResourceManager Get
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
        private List<UIResourceMapping> uiResourceMappings;
        
        private Dictionary<ResourceType, float> _resourceBank;
        private Dictionary<ResourceType, float> _resourceDeltaTracker;
        
        private ResourceType[] _globalResourceDefinitions;

        private float _updateDelaySeconds = 1f;
        private float _currentCountdown;

        public void Awake()
        {
            if (!CreateStaticInstance())
                return;
            
            LoadData();
            
            _currentCountdown = _updateDelaySeconds;
            _resourceBank = new Dictionary<ResourceType, float>();
            _resourceDeltaTracker = new Dictionary<ResourceType, float>();

            foreach (var resourceType in _globalResourceDefinitions)
            {
                _resourceBank.Add(resourceType, 0);
                _resourceDeltaTracker.Add(resourceType, 0);
            }
        }

        private void LoadData()
        {
            _globalResourceDefinitions = Resources.LoadAll<ResourceType>("Data");
        }

        public void AddResourceBankValue(ResourceType type, float amount)
        {
            Debug.Assert(_resourceBank.ContainsKey(type), $"Resource not present in resource array: {type.Name}");
            _resourceBank[type] += amount;
        }
        
        public void RemoveResourceBankValue(ResourceType type, float amount)
        {
            Debug.Assert(_resourceBank.ContainsKey(type), $"Resource not present in resource array: {type.Name}");
            _resourceBank[type] -= amount;
        }

        public void SetExplicitResourceBankValue(ResourceType type, float amount)
        {
            Debug.Assert(_resourceBank.ContainsKey(type), $"Resource not present in resource array: {type.Name}");
            _resourceBank[type] = amount;
        }

        public void Update()
        {
            _currentCountdown -= Time.deltaTime;
            if (!(_currentCountdown <= 0)) 
                return;
            
            _currentCountdown = _updateDelaySeconds;
            UpdateResourceAmounts();
                
            UpdateUI();
        }

        private void UpdateResourceAmounts()
        {
            // Clear the resource trackers. 
            foreach (var type in _globalResourceDefinitions)
            {
                if (_resourceDeltaTracker.ContainsKey(type))
                {
                    _resourceDeltaTracker[type] = 0;
                }
            }

            // Iterate each unit type once, and calculate it's entire population contribution. 
            foreach (var populationUnit in PopulationManager.Get.GathererPopulation)
            {
                var multiplier = populationUnit.Value;
                foreach (var modifier in populationUnit.Key.ResourceModifiers)
                {
                    var type = modifier.Resource;
                    Debug.Assert(_resourceBank.ContainsKey(type),
                        $"Resource not present in resource array: {type.Name}");

                    if (!_resourceDeltaTracker.ContainsKey(type))
                    {
                        _resourceDeltaTracker.Add(type, modifier.Amount * multiplier);
                    }
                    else
                    {
                        _resourceDeltaTracker[type] += modifier.Amount * multiplier;
                    }
                }
            }
            
            // Add any seasonal modifiers
            var season = SeasonManager.Get.CurrentSeason;
            foreach (var globalMod in season.globalResourceModifiers)
            {
                switch (globalMod.ModifierType)
                {
                    case EResourceAffectType.Gain:
                        // Ignore
                        break;
                    case EResourceAffectType.Overhead:
                        // Ignore
                        break;
                    case EResourceAffectType.MultiGlobal:
                        _resourceDeltaTracker[globalMod.Resource] *= globalMod.Amount;
                        break;
                    case EResourceAffectType.OverheadPerPop:
                        _resourceDeltaTracker[globalMod.Resource] += globalMod.Amount * PopulationManager.Get.TotalPopulation;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Finally tally the amounts
            foreach (var resourceType in _globalResourceDefinitions)
            {
                _resourceBank[resourceType] += _resourceDeltaTracker[resourceType];
                if (!resourceType.CanGoNegative && _resourceBank[resourceType] < 0)
                {
                    _resourceBank[resourceType] = 0;
                }
            }
        }

        private void UpdateUI()
        {
            foreach (var mapping in uiResourceMappings)
            {
                Debug.Assert(_resourceBank.ContainsKey(mapping.targetType),
                    $"Resource not present in _resourceBank: {mapping.targetType.Name}");
                Debug.Assert(_resourceDeltaTracker.ContainsKey(mapping.targetType),
                    $"Resource not present in _resourceDeltas: {mapping.targetType.Name}");
                
                var delta = Mathf.Floor(_resourceDeltaTracker[mapping.targetType] * 10) / 10;
                var total = Mathf.FloorToInt(_resourceBank[mapping.targetType]);
                
                mapping.textObjectDelta.text = delta >= 0 ? "+" + delta : delta.ToString();
                mapping.textObjectTotal.text = total.ToString();

                mapping.textObjectDelta.color = delta > 0 ? Color.green : Color.red;
                mapping.textObjectTotal.color = total > 0 ? Color.green : Color.red;
                mapping.objectIcon.sprite = mapping.targetType.Icon;
            }
        }
    }
}
