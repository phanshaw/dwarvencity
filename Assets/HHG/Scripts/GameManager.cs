using System;
using System.Collections;
using System.Collections.Generic;
using HHG.Scripts;
using UnityEngine;

[Serializable]
public class GameManager : MonoBehaviour
{
    // Singleton management. Can I abstract this?
    private static GameManager instance;
    public static GameManager Get
    {
        get { return instance; }
    }

    private bool CreateStaticInstance()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return false;
        }

        instance = this;
        return true;
    }

    [SerializeField]
    private GameScenarioDefinition _gameScenario;
    public GameScenarioDefinition GameScenario => _gameScenario;

    private void Awake()
    {
        if (!CreateStaticInstance())
            return;
    }

    public void Start()
    {
        SetGameScenario();
    }

    public void SetGameScenario()
    {
        foreach (var resourceQuantityMap in _gameScenario.StartingResources)
        {
            ResourceManager.Get.SetExplicitResourceBankValue(resourceQuantityMap.Type, resourceQuantityMap.Amount);
        }

        foreach (var populationQuantityMap in _gameScenario.StartingUnitTypes)
        {
            for(var i = 0; i < populationQuantityMap.Amount; i++) 
                PopulationManager.Get.SpawnUnit(populationQuantityMap.Type, Vector3.zero, Quaternion.identity);
        }
    }
}
