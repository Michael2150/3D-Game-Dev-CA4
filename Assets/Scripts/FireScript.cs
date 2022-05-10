using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour, IInteractable
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private ParticleSystem fire;
    [SerializeField] private AudioClip fireSound;
    private AudioSource audioSource;
    [SerializeField] private bool isOn = false;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    public void Interact(GameObject sender)
    {
        if (!isOn)
        {
            //Play the explosion particle system
            explosion.Play();
            //Play the explosion sound
            AudioSource.PlayClipAtPoint(explosionSound, transform.position, 0.5f);
        
            //Play the fire particle system
            fire.Play();
            //Play the fire sound looping
            audioSource.clip = fireSound;
            audioSource.loop = true;
            audioSource.Play();
            
            isOn = true;
        }
    }

    public string getInteractableText()
    {
        return "Press E to light the fire";
    }
}
