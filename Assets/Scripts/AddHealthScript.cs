using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddHealthScript : MonoBehaviour, IInteractable
{
    [SerializeField] private int healthToAdd = 5;
    
    public void Interact(GameObject sender)
    {
        //if the sender has the PlayerHealth script
        var playerHealth = sender.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            //remove health
            playerHealth.Health += healthToAdd;
    }
}
