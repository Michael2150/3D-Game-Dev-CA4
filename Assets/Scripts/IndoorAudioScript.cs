using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndoorAudioScript : MonoBehaviour
{
    private AudioSource _audioSource;
    private BoxCollider _boxCollider;
    private float volume = 0.15f;
    private float fadeSpeed = 0.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Fade in the audio source if the player is inside the collider
        if (other.gameObject.tag == "Player")
        {
            _audioSource.volume = 0;
            _audioSource.Play();
            StartCoroutine(FadeIn());
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        //Fade out the audio source if the player is outside the collider
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(FadeOut());
        }
    }
    
    private IEnumerator FadeIn()
    {
        while (_audioSource.volume < volume)
        {
            _audioSource.volume += fadeSpeed * Time.deltaTime;
            yield return null;
        }
    }
    
    private IEnumerator FadeOut()
    {
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= fadeSpeed * Time.deltaTime;
            yield return null;
        }
    }
}