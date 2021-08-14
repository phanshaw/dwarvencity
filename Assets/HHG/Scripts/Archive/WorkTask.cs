public struct WorkTask
{
    public int RemainingWork;

    public WorkTask(int workCost)
    {
        RemainingWork = workCost;
    }

    public void DoWork(int workAmount)
    {
        RemainingWork -= workAmount;
    }

    public bool IsComplete()
    {
        return RemainingWork <= 0;
    }
}