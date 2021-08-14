using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWorkStatus
{
    Complete,
    Cooldown,
    Failed,
    Inprogress
}
 
[CreateAssetMenu(fileName = "WorkshopType", menuName = "HHG/Data/New Workshop Definition", order = 1)]
public class WorkshopType : ScriptableObject
{
    [SerializeField]
    private List<WorkType> allowedWorkTypes;

    [SerializeField]
    private int maxQueueLength = 5;

    public int MaxQueueLength => maxQueueLength;
}
