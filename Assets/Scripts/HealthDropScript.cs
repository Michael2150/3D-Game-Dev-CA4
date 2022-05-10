using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDropScript : MonoBehaviour, IInteractable
{
    [SerializeField] private int healthToAdd = 20;

    public void Interact(GameObject sender)
    {
        //if the sender has the PlayerHealth script
        var playerHealth = sender.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            //add health
            playerHealth.Health += healthToAdd;
        
        Destroy(gameObject);
    }

    public string getInteractableText()
    {
        return $"Press E to pick up {healthToAdd}% health";
    }
}
