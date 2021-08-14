using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.BehaviourTrees;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Scheduler : MonoBehaviour
{
    public List<Worker> workers;
    public List<Workshop> workshops;

    public WorkType debugWorkType;
    
    public void Start()
    {
        // TODO: This is a real weird spot to initialize this. It's kinda something that should
        // probably be done via the workshops themselves. 
        foreach (var workshop in workshops)
        {
            workshop.QueueWorkOrder(new WorkOrder(5, debugWorkType));
        }

        foreach (var worker in workers)
        {
            worker.SetScheduler(this);
        }
    }
    
    public Workshop GetUnassignedWorkshop()
    {
        // TODO: Probably have an agent's preferred work type here somewhere...
        
        foreach (var workshop in workshops)
        {
            if(workshop.IsReserved)
                continue;
            
            if (workshop.HasActiveWorkOrder())
            {
                return workshop;
            }
        }

        return null;
    }
}
