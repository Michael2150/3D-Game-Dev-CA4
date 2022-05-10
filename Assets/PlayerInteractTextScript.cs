using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractTextScript : MonoBehaviour
{
    [SerializeField] private Text interactText;
    [SerializeField] private PlayerInteractingScript playerInteractingScript;
    private Coroutine showTextCoroutine = null;

    private void Start()
    {
        //Start a coroutine that acts as an update every 0.1 seconds
        showTextCoroutine = StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        //Do the update
        while (true)
        {
            doUpdate();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void doUpdate()
    {
        //Check if there is any object with the tag "Ammo" in the radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, playerInteractingScript.InteractRadius);
        bool foundInteractable = false;
        foreach (Collider collider in colliders)
        {
            //Check if the object has an IInteractable interface
            var interactable = collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                foundInteractable = true;
                interactText.text = interactable.getInteractableText();
                break;
            }
        }

        if (!foundInteractable)
            interactText.text = "";
    }
    
    private IEnumerator ShowPickupText(string text)
    {
        interactText.text = text;
        yield return new WaitForSeconds(3);
        interactText.text = "";
        showTextCoroutine = null;
    }
}