using System.Collections.Generic;
using UnityEngine;

namespace HHG.Scripts
{
    [CreateAssetMenu(fileName = "TrainUnit", menuName = "HHG/Data/New Train Unit Type", order = 1)]
    public class InteractionBehaviorConvertUnit : InteractionBehavior
    {
        [SerializeField] 
        private UnitDefinition _outputUnitType;
        public UnitDefinition OutputUnitType => _outputUnitType;
        
        [SerializeField] 
        private List<ResourceModifier> _conversionCosts;
        public List<ResourceModifier> ConversionCosts => _conversionCosts;
    
        public override void ExecuteAction(InteractionComponent instigator)
        {
            // Check the costs.
            if (_conversionCosts.Count > 0)
            {
                // TODO: Check the resource manager...    
            }
            
            // Check the incoming object is the right type...
            var unitComponent = instigator.GetComponent<UnitComponent>();
            if(unitComponent == null)
                return;
            
            // Nothing to do here if the unit is already the right type...
            if(unitComponent.UnitType == _outputUnitType)
                return;

            PopulationManager.Get.ConvertUnitType(instigator.gameObject, instigator.Unit.UnitType, OutputUnitType);
        }
    }
}
