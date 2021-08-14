using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace HHG.Scripts.Actions
{
    [Name("Interact With (Interaction Component)")]
    [Category("HHG/AI/Interaction")]
    public class InteractWithObject : ActionTask<InteractionComponent>
    {
        [RequiredField]
        public BBParameter<InteractionComponent> target;
        
        protected override string info 
        {
            get { return "Interact " + target; }
        }
        
        protected override void OnExecute() 
        {
            if (target.value == null)
            {
                EndAction(false); 
                return;
            }
            
            agent.InteractWith(target.value);
        
            EndAction(true);
        }
    }
}
