using System;
using UnityEngine;

public interface IInteractable
{
    public void Interact(GameObject sender);
    
    public String getInteractableText();
}
