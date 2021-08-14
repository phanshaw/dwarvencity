using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

using NavMesh = UnityEngine.AI.NavMesh;
using NavMeshHit = UnityEngine.AI.NavMeshHit;

namespace HHG.Scripts
{

    [Name("Do work")]
    [Category("Worker")]
    [Description("Do work at your workshop.")]
    public class DoWork : ActionTask
    { 
        [BlackboardOnly]
        public BBParameter<Workshop> Workshop;

        [BlackboardOnly]
        public BBParameter<int> Status;
        
        protected override void OnExecute() 
        {
            if (Workshop.value == null)
            {
                EndAction(false);
            }
            
            var result = Workshop.value.DoWork();
            if (result == EWorkStatus.Complete)
            {
                Workshop.value = null;
                Status.SetValue((int)EWorkerStatus.Idle);
                EndAction(false);
            }

            EndAction(true);
        }
    }
}