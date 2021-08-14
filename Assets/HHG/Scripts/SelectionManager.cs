using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace HHG.Scripts
{
    
    [Serializable]
    public class SelectionManager : MonoBehaviour
    {
        // Singleton management. Can I abstract this?
        private static SelectionManager _instance;
        public static SelectionManager Get
        {
            get { return _instance; }
        }

        private bool CreateStaticInstance()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return false;
            }

            _instance = this;
            return true;
        }

        private HashSet<InteractionComponent> _interactionComponents;
        public HashSet<InteractionComponent> InteractionComponents => _interactionComponents;
        
        private void Awake()
        {
            if (!CreateStaticInstance())
                return;

            _interactionComponents = new HashSet<InteractionComponent>();
        }

        private void Update()
        {
            // What can we do?
            
            // Left click:
            // Has selection?
            // Deselect and select whats under the mouse
            // If nothing under mouse, deselect all...
            
            // Right click:
            // Give unit contextual order
            // Is valid target?
            // Object type?
            // Friendly?
            
            if (Input.GetMouseButtonDown(1))
            {
                SetContextualTarget();
            }
            
            // TODO: Polish idea... if nothing selectable... poke the world?
        }

        public void SetContextualTarget()
        {
            // First we need to figure out what our target is.
            // Do a raycast and see what we see! 
            // TODO: Probably need a camera manager...

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                var objectHit = hit.transform;

                // Do something with the object that was hit by the raycast.
                var interactionComponent = objectHit.GetComponent<InteractionComponent>();
                if (interactionComponent != null)
                {
                    SetInteractionComponentAsTarget(interactionComponent);
                    return;
                }
                
                // If it's just the ground..
                if (NavMesh.SamplePosition(hit.point, out var navMeshHit, 1f, NavMesh.AllAreas))
                {
                    SetWorldPositionAsTarget(hit.point);
                }
            }
        }

        private void SetWorldPositionAsTarget(Vector3 targetLocation)
        {
            Debug.Log("Moving to world position.");
            foreach (var interactionComponent in _interactionComponents)
            {
                interactionComponent.SetMoveTarget(targetLocation);
            }
        }

        private void SetInteractionComponentAsTarget(InteractionComponent targetComponent)
        {
            Debug.Log("Interacting with object.");
            foreach (var interactionComponent in _interactionComponents)
            {
                if(interactionComponent == targetComponent)
                    continue;
                
                interactionComponent.SetInteractionTarget(targetComponent);
                interactionComponent.SetMoveTarget(targetComponent.transform.position);
            }
        }

        public void RemoveFromSelection(InteractionComponent target)
        {
            if(target == null)
                return;
            
            if (_interactionComponents.Contains(target))
            {
                target.Deselect();
                _interactionComponents.Remove(target);
            }
        }

        public void Select(InteractionComponent target, bool clear=true)
        {
            if (clear)
            {
                foreach (var interactionComponent in _interactionComponents)
                {
                    interactionComponent.Deselect();
                }
            }

            _interactionComponents.Clear();
            _interactionComponents.Add(target);
        }

        public void SelectMulti(List<InteractionComponent> targets, bool clear = true)
        {
            
        }
    }
}
