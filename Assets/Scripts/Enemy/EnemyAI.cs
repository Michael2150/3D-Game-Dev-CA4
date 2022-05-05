using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.Serialization;

public class EnemyAI : MonoBehaviour, IHittable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float currentHealth;
    
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
                return; //If the enemy is dead, do nothing
            
            case EnemyState.Roaming:
                enemyAIMovement.updateMovement(); //Update the movement
                
                //If the enemy can see the player, change state to chasing
                if (enemyAIMovement.KnowTargetPosition)
                    State = EnemyState.Chasing;
                break;
            
            case EnemyState.Chasing:
                enemyAIMovement.updateMovement(); //Update the movement
                
                //If the enemy does not see the player anymore, and it has reached the last known position, go back to roaming
                if (!enemyAIMovement.KnowTargetPosition && enemyAIMovement.DestinationReached) 
                    State = EnemyState.Roaming;

                //If the enemy can see the player, change state to attacking
                if (enemyAIMovement.withinAttackRange)
                    State = EnemyState.Attacking;
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

    public float Health
    {
        get => currentHealth;
        set
        {
            //Make sure the health is between 0 and 100
            currentHealth = Mathf.Clamp(value, 0f, 100f);
            
            //If the player health is 0, then the player is dead
            if (currentHealth == 0)
                State = EnemyState.Dead;
        }
    }

    public void Hit(GameObject hittingObject, IHitter hitter)
    {
        //If the hitting object is the player, then the player is hit
        if (hittingObject.CompareTag("Player"))
            Health -= hitter.getHitDamage();
    }
}
