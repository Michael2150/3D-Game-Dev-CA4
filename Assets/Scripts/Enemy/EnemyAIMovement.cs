using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAIMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] public Transform target;
    
    [Space] [Header("Roaming Attributes")]
    [SerializeField] public float Roaming_RandomRadius = 3f;
    
    [Space] [Header("Roaming")]
    [SerializeField] public float Roaming_speed = 4f;
    [SerializeField] public float Roaming_rotationSpeed = 120f;
    [SerializeField] public float Roaming_fovAngle = 170f;
    [SerializeField] public float Roaming_fovRadius = 20f;
    [SerializeField] public float Roaming_closeRadius = 2f;

    [Space] [Header("Chasing")]
    [SerializeField] public float Chasing_speed = 5f;
    [SerializeField] public float Chasing_rotationSpeed = 140f;
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

        //Check if the player is within the fov radius
        return isTargetWithinFOV;
    }
    
    public bool isTargetWithinFOV
    {
        get
        {
            float distance = Vector3.Distance(target.position, transform.position);
            
            //Check if the player is in the field of view
            Vector3 targetDirection = target.position - transform.position;
            float targetAngle = Vector3.Angle(targetDirection, transform.forward);
            bool isInFOV = (targetAngle <= fovAngle / 2); 
        
            //Check if the player is within the field of view radius
            bool isInFOVRadius = (distance <= fovRadius);
        
            return isInFOV && isInFOVRadius;
        }
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
            
            fovAngle = (_isChasing) ? Chasing_fovAngle : Roaming_fovAngle;
            fovRadius = (_isChasing) ? Chasing_fovRadius : Roaming_fovRadius;
            closeRadius = (_isChasing) ? Chasing_closeRadius : Roaming_closeRadius;
            agent.speed = (_isChasing) ? Chasing_speed : Roaming_speed;
            agent.angularSpeed = (_isChasing) ? Chasing_rotationSpeed : Roaming_rotationSpeed;
            
            if (!_isChasing)
                RoamDestination = Vector3.zero;
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
        get => isChasing //If the enemy is chasing the player 
             ? KnowTargetPosition  //If the target is known
                ? target.position  //Return the target position
                : lastKnownTargetPosition //Return the last known position of the target
             : RoamDestination; //If the enemy is not chasing the player, return the RoamDestination
    }

    private Vector3 _roamDest = Vector3.zero;
    public Vector3 RoamDestination
    {
        get
        {
            if (_roamDest == Vector3.zero || DestinationReached)
                RoamDestination = randomNavMeshPoint(Roaming_RandomRadius);
            return _roamDest;
        }
        set => _roamDest = value;
    }

    //Check if the enemy is in the attackRange and within the fov of the enemy
    public bool withinAttackRange { get => (Vector3.Distance(target.position, transform.position) <= attackRange) && isTargetWithinFOV; }

    private Vector3 randomNavMeshPoint(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1)) {
            finalPosition = hit.position;            
        }
        return finalPosition;
    }
}
