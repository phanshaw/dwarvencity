using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace NodeCanvas.Tasks.Actions
{
    using UnityEngine;
    using UnityEngine.AI;

    [Category("Worker")]
    [Description("Move to and occupy a given workshop. ")]
    public class OccupyWorkshop : ActionTask<NavMeshAgent>
    {
        [RequiredField]
        public BBParameter<Workshop> targetWorkshop;

        public BBParameter<float> speed = 4;
        public BBParameter<float> keepDistance = 0.1f;

        private Vector3? lastRequest;

        protected override string info => "Seek Workshop";

        protected override void OnExecute()
        {
            var pos = Vector3.zero;
            if (!GetWorkLocatorPosition(ref pos)) 
                return;
            
            agent.speed = speed.value;
            if (Vector3.Distance(agent.transform.position, pos) < agent.stoppingDistance + keepDistance.value)
            {
                EndAction(true);
            }
        }

        protected override void OnUpdate()
        {
            var pos = Vector3.zero;
            if (GetWorkLocatorPosition(ref pos))
            {
                if (lastRequest != pos)
                {
                    if (!agent.SetDestination(pos))
                    {
                        EndAction(false);
                        return;
                    }
                }

                lastRequest = pos;

                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + keepDistance.value)
                {
                    EndAction(true);
                }
            }
            else
            {
                EndAction(false);
            }
        }


        protected override void OnPause()
        {
            OnStop();
        }

        protected override void OnStop()
        {
            if (lastRequest != null && agent.gameObject.activeSelf)
            {
                agent.Warp(agent.transform.position);
                agent.ResetPath();
            }

            lastRequest = null;
        }

        public override void OnDrawGizmosSelected()
        {
            var pos = Vector3.zero;
            if(GetWorkLocatorPosition(ref pos))
            {
                Gizmos.DrawWireSphere(pos, keepDistance.value);
            }
        }

        private bool GetWorkLocatorPosition(ref Vector3 position)
        {
            var targetValue = targetWorkshop.value;
            if (targetValue == null)
            {
                return false;
            }

            if (targetValue.WorkLocator == null)
            {
                return false;
            }

            position = targetValue.WorkLocator.position;
            return true;
        }
    }
}
