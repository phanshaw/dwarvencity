using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.BehaviourTrees;
using UnityEngine;

public class Workshop : MonoBehaviour
{
    [SerializeField]
    private WorkshopType workshopType;
    public WorkshopType WorkshopType => workshopType;

    [SerializeField]
    public Transform WorkLocator;

    [SerializeField]
    private ParticleSystem workIndicator;
    
    private BehaviourTreeOwner worker = null;

    public void Start()
    {
        _workOrders = new Stack<WorkOrder>();
    }

    public bool ReserveForWorker(BehaviourTreeOwner newWorker, bool force = false)
    {
        if (worker == null || force)
        {
            worker = newWorker;
            return true;
        }

        return false;
    }

    public void ClearWorker()
    {
        worker = null;
    }

    public bool IsReserved => worker != null;
    
    private Stack<WorkOrder> _workOrders;
    private WorkOrder activeWorkOrder;

    public bool HasActiveWorkOrder()
    {
        return activeWorkOrder.IsValid;
    }

    public bool QueueWorkOrder(WorkOrder workOrder)
    {
        if (_workOrders == null)
            return false;

        // Start the task immediately
        if (!activeWorkOrder.IsValid)
        {
            activeWorkOrder = workOrder;
            return true;
        }
        else if(_workOrders.Count < workshopType.MaxQueueLength)
        {
            _workOrders.Push(workOrder);
            return true;
        }

        return false;
    }

    private bool SetNextWorkOrder()
    {
        if (_workOrders.Count == 0)
            return false;
        activeWorkOrder = _workOrders.Pop();
        return true;
    }

    public EWorkStatus DoWork()
    {
        var status = EWorkStatus.Complete;
        if (HasActiveWorkOrder())
        {
            status = activeWorkOrder.DoWork();
            if (status == EWorkStatus.Inprogress)
            {
                workIndicator.Clear();
                workIndicator.Play();
            }
        }

        // TODO: Check to see if we have more work to do...
        if (SetNextWorkOrder())
        {
            status = activeWorkOrder.DoWork();
            
            workIndicator.Clear();
            workIndicator.Play();
        }
        
        // TODO: Handle the status here...
        return status;
    }
}
