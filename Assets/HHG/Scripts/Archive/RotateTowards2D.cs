namespace HHG.Scripts
{
    using NodeCanvas.Framework;
    using ParadoxNotion.Design;
    using UnityEngine;

    [Category("Movement/Direct")]
    [Description("Rotate the agent towards the target per frame on the Y axis")]
    public class RotateTowards2D : ActionTask<Transform>
    {

        [RequiredField]
        public BBParameter<GameObject> target;

        public BBParameter<float> speed = 2;

        [SliderField(1, 180)]
        public BBParameter<float> angleDifference = 5;

        public BBParameter<Vector3> upVector = Vector3.up;
        public bool waitActionFinish;

        protected override void OnUpdate()
        {
            var targetPosition = target.value.transform.position;
            var agentPos2D = new Vector3(agent.position.x, 0, agent.position.z);
            var targetPos2D = new Vector3(targetPosition.x, 0, targetPosition.z);

            if (Vector3.Angle(targetPos2D - agentPos2D, agent.forward) <= angleDifference.value)
            {
                EndAction();
                return;
            }

            var dir = targetPos2D - agentPos2D;
            agent.rotation = Quaternion.LookRotation(Vector3.RotateTowards(agent.forward, dir, speed.value * Time.deltaTime, 0), upVector.value);
            if (!waitActionFinish)
            {
                EndAction();
            }
        }
    }
}