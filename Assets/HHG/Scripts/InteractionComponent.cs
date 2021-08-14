using System;
using HHG.Scripts.Utils;
using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace HHG.Scripts
{
    public class InteractionComponent : MonoBehaviour
    {
        private const string MOVE_TARGET_PARAM_NAME = "moveTarget";
        private const string INTERACTION_TARGET_PARAM_NAME = "interactionTarget";
        private const string HAS_MOVE_TARGET_PARAM_NAME = "hasMoveTarget";

        [Header("Cached Component References")]
        [SerializeField] 
        private UnitComponent unit;
        public UnitComponent Unit => unit;
        
        [SerializeField] 
        private MeshRenderer _decalProjector;
        
        [SerializeField] 
        private BehaviourTreeOwner _behaviourTreeOwner;
        
        [SerializeField] 
        private InteractionBehavior unitInteractionBehavior;
        public InteractionBehavior UnitInteractionBehavior => unitInteractionBehavior;

        private Color _debugDrawColor;
        
        private bool _hovered;

        private void Awake()
        {
            _debugDrawColor = ColorUtils.Random();
            Debug.Assert(_decalProjector != null, $"Unit {name} has null decal projector");
            _decalProjector.enabled = false;
        }

        public void SetInteractionTarget(InteractionComponent other)
        {
            if (_behaviourTreeOwner == null) 
                return;

            _behaviourTreeOwner.graph.blackboard.SetVariableValue(INTERACTION_TARGET_PARAM_NAME, other);
        }

        public void SetMoveTarget(Vector3 targetPosition)
        {
            if (_behaviourTreeOwner == null) 
                return;
            
            _behaviourTreeOwner.graph.blackboard.SetVariableValue(MOVE_TARGET_PARAM_NAME, targetPosition);
            _behaviourTreeOwner.graph.blackboard.SetVariableValue(HAS_MOVE_TARGET_PARAM_NAME, true);
            _behaviourTreeOwner.RestartBehaviour();
        }

        private void OnMouseEnter()
        {
            _hovered = true;
            Debug.Log($"Hovered {name}");
        }

        private void OnMouseExit()
        {
            _hovered = false;
        }

        private void Update()
        {
            // Select
            if (_hovered && Input.GetMouseButtonDown(0))
            {
                Select(true);
            }
        }

        public void InteractWith(InteractionComponent other)
        {
            if (other == null)
                return;
            
            Debug.Log($"{name} interacting with {other.name}");
            other.UnitInteractionBehavior.ExecuteAction(this);
        }

        public void Select()
        {
            Select(false);
        }

        public void Select(bool clearSelection)
        {
            if (SelectionManager.Get.InteractionComponents.Contains(this)) 
                return;

            SelectionManager.Get.Select(this, true);
            _decalProjector.enabled = true;
        }

        public void Deselect()
        {
            Debug.Log($"Deselected {name}");
            _decalProjector.enabled = false;
        }
    }
}
