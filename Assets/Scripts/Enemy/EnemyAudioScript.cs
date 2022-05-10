using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioScript : MonoBehaviour
{
    private EnemyAI enemyAiScript;
    private AudioSource audioSource;
    [SerializeField] AudioClip[] enemySounds;
    [SerializeField] AudioClip enemyAttackSound;
    private EnemyAI.EnemyState lastState;
    private Coroutine playSoundCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        enemyAiScript = GetComponent<EnemyAI>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        if (enemyAiScript.State != lastState)
        {
            //Stop the coroutine if it is running
            if (playSoundCoroutine != null)
            {
                StopCoroutine(playSoundCoroutine);
                playSoundCoroutine = null;
            }

            //if the enemy state has changed to dead or attacking, play the sound only once
            if (enemyAiScript.State == EnemyAI.EnemyState.Dead)
                audioSource.PlayOneShot(enemySounds[0]);
            else if (enemyAiScript.State == EnemyAI.EnemyState.Attacking)
                audioSource.PlayOneShot(enemyAttackSound);
        }
        else
        {
            if (playSoundCoroutine == null)
                playSoundCoroutine = StartCoroutine(PlaySound());
        }

        lastState = enemyAiScript.State;
    }

    private IEnumerator PlaySound()
    {
        if (enemyAiScript.State != EnemyAI.EnemyState.Dead)
        {
            //Wait a random amount of time between 2 and 10 seconds and then play a random zombie sound.
            yield return new WaitForSeconds(Random.Range(2, 10));
            audioSource.PlayOneShot(enemySounds[Random.Range(1, enemySounds.Length)]);
            //Set the pitch to a random value between 0.8 and 1.2
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            //Set the speed to a random value between 0.8 and 1.2
            audioSource.volume = Random.Range(0.8f, 1.2f);
        
            //Start the coroutine again
            playSoundCoroutine = StartCoroutine(PlaySound());    
        }
    }

}
