

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

using NavMesh = UnityEngine.AI.NavMesh;
using NavMeshHit = UnityEngine.AI.NavMeshHit;

namespace HHG.Scripts
{
    using NodeCanvas.BehaviourTrees;

    [Name("Find Task")]
    [Category("Worker")]
    [Description("Find a workshop with an active work order.")]
    public class GetWorkFromScheduler : ActionTask
    {
        [BlackboardOnly]
        public BBParameter<Scheduler> Scheduler;
        
        [BlackboardOnly]
        public BBParameter<Workshop> Workshop;

        [BlackboardOnly]
        public BBParameter<int> Status;
        
        protected override void OnExecute() 
        {
            if (!Workshop.isNull)
            {
                EndAction(true);
            }

            if (!Scheduler.isNull)
            {
                var workshop = Scheduler.value.GetUnassignedWorkshop();
                if (workshop != null)
                {
                    var btOwner = agent.GetComponent<BehaviourTreeOwner>();
                    workshop.ReserveForWorker(btOwner);

                    Workshop.value = workshop;
                    Status.SetValue((int)EWorkerStatus.Working);
                    EndAction(true);
                }
            }

            EndAction(false);
        }
    }
}
