using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace HHG.Scripts
{
    [Serializable]
    public struct UISeasonMapping
    {
        public Image objectIcon;
        public Text seasonName;
    }
    
    [Serializable]
    public class SeasonManager : MonoBehaviour
    {
        // Singleton management. Can I abstract this?
        private static SeasonManager _instance;
        public static SeasonManager Get
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
        private UISeasonMapping uiSeasonMappings;

        [SerializeField]
        private List<SeasonDefinition> seasonDefinitions;

        [SerializeField] 
        private int yearCounter;
        
        private SeasonDefinition _currentSeason;
        public SeasonDefinition CurrentSeason => _currentSeason;
        
        private float _currentCountdown;
        
        public void Awake()
        {
            if (!CreateStaticInstance())
                return;

            yearCounter = 1;
            _currentSeason = seasonDefinitions[0];
            _currentCountdown = _currentSeason.DurationSeconds;
            UpdateUI();
        }

        private void Update()
        {
            _currentCountdown -= Time.deltaTime;
            if (!(_currentCountdown <= 0)) 
                return;
            
            // Change the season!
            var nextIndex = (seasonDefinitions.IndexOf(_currentSeason) + 1) % seasonDefinitions.Count;
            _currentSeason = seasonDefinitions[nextIndex];
            _currentCountdown = _currentSeason.DurationSeconds;
            
            // Tick a year over
            if(nextIndex == 0)
                yearCounter++;
            
            // TODO: Add seasonal influences to the resource manager. Maybe an event that the 
            // season has changed. 
            
            UpdateUI();
        }

        private void UpdateUI()
        {
            uiSeasonMappings.objectIcon.sprite = _currentSeason.icon;
            uiSeasonMappings.seasonName.text = $"{_currentSeason.seasonName}, year {yearCounter}";
        }
    }
}
