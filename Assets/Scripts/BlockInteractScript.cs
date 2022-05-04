using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BlockInteractScript : MonoBehaviour, IInteractable
{
    public void Interact(GameObject sender)
    {
        Debug.Log("Interacted by: " + sender);
        Destroy(gameObject);
    }
}
