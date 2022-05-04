using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractingScript : MonoBehaviour, IInteractor
{
    [SerializeField] private float _interactRadius = 2f;

    public float InteractRadius => _interactRadius;

    public List<IInteractable> GetInteractablesWithinRadius()
    {
        //Get all the IInteractable objects in the radius
        List<IInteractable> interactables = new List<IInteractable>();
        Collider[] colliders = Physics.OverlapSphere(transform.position, _interactRadius);
        foreach (Collider collider in colliders)
        {
            IInteractable interactable = collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactables.Add(interactable);
            }
        }
        
        return interactables;
    }

    public GameObject InteractWithFirstInteractable()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _interactRadius);
        foreach (Collider collider in colliders)
        {
            //Get all the Components from the collider's gameobject
            IInteractable interactable = collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(this.gameObject);
                return collider.gameObject;
            }
        }

        return null;
    }
}
