using System;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{
    public BehaviourTreeOwner btOwner;

    private void Start()
    {
    }

    public void SetScheduler(Scheduler scheduler)
    {
        btOwner.blackboard.SetVariableValue("Scheduler", scheduler);
    }
}
