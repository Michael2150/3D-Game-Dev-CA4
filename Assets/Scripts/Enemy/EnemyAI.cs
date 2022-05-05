using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Serialization;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    
    public enum EnemyState {Roaming, Chasing, Attacking, Dead};
    private EnemyState _state;

    [SerializeField] EnemyAIMovement enemyAIMovement;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        State = EnemyState.Roaming;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case EnemyState.Dead:
                return;
            case EnemyState.Roaming:
                enemyAIMovement.updateMovement();
                break;
            case EnemyState.Chasing:
                enemyAIMovement.updateMovement();
                break;
            case EnemyState.Attacking:
                break;
        }
    }

    public EnemyState State
    {
        get => _state;
        set
        {
            _state = value;
            
            switch (State)
            {
                case EnemyState.Dead:
                    return;
                case EnemyState.Roaming:
                    enemyAIMovement.isChasing = false;
                    break;
                case EnemyState.Chasing:
                    enemyAIMovement.isChasing = true;
                    break;
                case EnemyState.Attacking:
                    break;
            }
        }
    }

    public int Health
    {
        get => currentHealth;
        set
        {
            //Make sure the health is between 0 and 100
            currentHealth = Mathf.Clamp(value, 0, 100);
            
            //If the player health is 0, then the player is dead
            if (currentHealth == 0)
                State = EnemyState.Dead;
        }
    }
}
