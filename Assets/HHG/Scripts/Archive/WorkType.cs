using System.Collections.Generic;
using HHG.Scripts;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkType", menuName = "HHG/Data/New Work Definition", order = 1)]
public class WorkType : ScriptableObject
{
    public int cost;
    public float cooldown = 1f;
    
    public List<ResourceType> ingredients;
    public ResourceType output;
}
