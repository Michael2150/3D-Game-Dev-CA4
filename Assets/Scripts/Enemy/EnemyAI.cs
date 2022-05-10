using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IHittable
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float currentHealth;
    
    public enum EnemyState {Roaming, Chasing, Attacking, Dead};
    private EnemyState _state;

    [SerializeField] EnemyAIMovement enemyAIMovement;
    [SerializeField] EnemyAttackScript enemyAttackScript;
    [SerializeField] Animator animator;

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
                enemyAIMovement.isDead = true;
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
                if (attackCoroutine == null)
                    attackCoroutine = StartCoroutine(Attack());
                break;
        }
    }

    public EnemyState State
    {
        get => _state;
        set
        {
            if (_state != value)
            {
                _state = value;
                switch (State)
                {
                    case EnemyState.Dead:
                        animator.SetTrigger("Die");
                        return;
                    case EnemyState.Roaming:
                        enemyAIMovement.isChasing = false;
                        animator.SetTrigger("Walk");
                        break;
                    case EnemyState.Chasing:
                        enemyAIMovement.isChasing = true;
                        animator.SetTrigger("Run");
                        break;
                    case EnemyState.Attacking:
                        animator.SetTrigger("Attack");
                        break;
                }
            }
        }
    }
    
    Coroutine attackCoroutine = null;
    //Attack the player coroutine
    IEnumerator Attack()
    {
        //Get the length of the attack animation
        float attackLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        
        //Wait half the attack length
        yield return new WaitForSeconds(attackLength / 2);
        
        //Run the player attack script
        enemyAttackScript.Attack(enemyAIMovement.attackRange);
        
        //Wait for the animation to finish
        yield return new WaitForSeconds(attackLength / 2);
        
        //Return to chasing state
        State = EnemyState.Chasing;
        
        //Reset the attack coroutine
        attackCoroutine = null;
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
        //Log the hitting object tag
        Debug.Log(hittingObject.tag);
        
        //Check if the hitting object is the player
        if (hittingObject.CompareTag("Player"))
            Health -= hitter.getHitDamage();
    }
}
