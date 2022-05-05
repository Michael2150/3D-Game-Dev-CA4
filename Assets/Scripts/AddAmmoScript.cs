using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAmmoScript : MonoBehaviour, IInteractable
{
    [SerializeField] private int ammoToAdd = 10;
    
    public void Interact(GameObject sender)
    {
        //Go through all the children in the sender recursively and find the PlayerWeaponScript
        PlayerWeaponScript playerWeaponScript = sender.GetComponentInChildren<PlayerWeaponScript>();
        if (playerWeaponScript)
            playerWeaponScript.ReserveAmmo += ammoToAdd;
    }
}
