using UnityEngine;

public struct WorkOrder
{
    private bool _workOrderValid;
    public bool IsValid
    {
        get
        {
            if (!_workOrderValid)
                return false;

            if (_workOrderComplete)
                return false;

            return true;
        }
    }
    private bool _workOrderComplete => _workTasksRemaining <= 0;

    private float _cooldown;
    private int _workTasksRemaining;
    private WorkType _type;
    private WorkTask _activeTask;

    public WorkOrder(int count, WorkType type)
    {
        _cooldown = 0f;
        _workOrderValid = true;
        _workTasksRemaining = count;
        _type = type;
        _activeTask = new WorkTask(_type.cost);
    }

    public EWorkStatus DoWork()
    {
        if (_workTasksRemaining > 0 && _activeTask.IsComplete())
        {
            Debug.Log("Starting new task!");
            _activeTask = new WorkTask(_type.cost);
        }

        if (_cooldown > 0)
        {
            Debug.Log("Cooldown!");
            _cooldown -= Time.deltaTime;
            return EWorkStatus.Cooldown;
        }

        Debug.Log("Doing Work!");

        _activeTask.DoWork(1);
        _cooldown = _type.cooldown;

        if (_activeTask.IsComplete())
        {
            Debug.Log("I completed a task!");
            CompleteTask();
            
            if(_workOrderComplete)
                return EWorkStatus.Complete;
        }

        return EWorkStatus.Inprogress;
    }

    private bool TryStartNewTask()
    {
        // TODO: Check ingredients here...
        return true;
    }

    private void CompleteTask()
    {
        _workTasksRemaining--;
        _cooldown = 0;
    }
}