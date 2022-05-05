using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] public Transform target;
    
    [Space] [Header("Roaming")]
    [SerializeField] public float Roaming_fovAngle = 180f;
    [SerializeField] public float Roaming_fovRadius = 20f;
    [SerializeField] public float Roaming_closeRadius = 2f;

    [Space] [Header("Chasing")]
    [SerializeField] public float Chasing_fovAngle = 120f;
    [SerializeField] public float Chasing_fovRadius = 30f;
    [SerializeField] public float Chasing_closeRadius = 3f;

    [Space] [Header("Debug")]
    //Readonly properties for debugging
    [SerializeField] public float fovAngle;
    [SerializeField] public float fovRadius;
    [SerializeField] public float closeRadius;
    [SerializeField] public float attackRange;
    
    //Start
    private void Start()
    {
        lastKnownTargetPosition = transform.position;
    }
    
    //Update
    public void updateMovement()
    {
        KnowTargetPosition = TargetPosKnown();
        agent.SetDestination(DestinationPos);
    }

    public bool TargetPosKnown()
    {
        //Check if the player is within the close radius
        float distance = Vector3.Distance(target.position, transform.position);
        bool isInCloseRadius = (distance <= closeRadius);
        
        if (isInCloseRadius)
            return true;

        //Check if the player is in the field of view
        Vector3 targetDirection = target.position - transform.position;
        float targetAngle = Vector3.Angle(targetDirection, transform.forward);
        bool isInFOV = (targetAngle <= fovAngle / 2); 
        
        //Check if the player is within the field of view radius
        bool isInFOVRadius = (distance <= fovRadius);
        
        return isInFOV && isInFOVRadius;
    }

    private Vector3 lastKnownTargetPosition;
    public bool KnowTargetPosition
    {
        get => lastKnownTargetPosition == Vector3.zero;
        set
         {
             if (value) //If the target is in the field of view, update the last known position of the target.
                 lastKnownTargetPosition = Vector3.zero;
             else //If the target is not in the field of view, update the last known position of the target only once.
                if (lastKnownTargetPosition == Vector3.zero)
                     lastKnownTargetPosition = target.position;
         }
    }
    
    //isChasing property, when the enemy is chasing the player, it will be true. And the Chasing properties will be used. Otherwise, the Roaming properties will be used.
    private bool _isChasing = false;
    public bool isChasing
    {
        get => _isChasing;
        set
        {
            _isChasing = value;
            if (_isChasing)
            {
                fovAngle = Chasing_fovAngle;
                fovRadius = Chasing_fovRadius;
                closeRadius = Chasing_closeRadius;
            }
            else
            {
                fovAngle = Roaming_fovAngle;
                fovRadius = Roaming_fovRadius;
                closeRadius = Roaming_closeRadius;
            }
        }
    }
    
    //Check if the Destination position has been reached
    public bool DestinationReached
    {
        get => ((!agent.pathPending)
                && (agent.remainingDistance <= agent.stoppingDistance)
                && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f));
    }
    
    public Vector3 DestinationPos
    {
        get => KnowTargetPosition ? target.position : lastKnownTargetPosition;
    }
}
