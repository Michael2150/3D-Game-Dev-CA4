using System.Collections.Generic;
using UnityEngine;

public interface IInteractor
{
    public float InteractRadius { get;}

    public List<IInteractable> GetInteractablesWithinRadius();
    
    public GameObject InteractWithFirstInteractableObject();
}
