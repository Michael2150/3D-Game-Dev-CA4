using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDropScript : MonoBehaviour, IInteractable
{
    [SerializeField] private int healthToAdd = 20;

    public void Interact(GameObject sender)
    {
        //Go through all the children in the sender recursively and find the PlayerWeaponScript
        PlayerWeaponScript playerWeaponScript = sender.GetComponentInChildren<PlayerWeaponScript>();
        if (playerWeaponScript)
            playerWeaponScript.ReserveAmmo += healthToAdd;
        
        Destroy(gameObject);
    }

    public string getInteractableText()
    {
        return $"Press E to pick up {healthToAdd}% health";
    }
}
