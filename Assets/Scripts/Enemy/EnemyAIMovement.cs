using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] public Transform target;
    
    [SerializeField] public float fovAngle = 90f;
    [SerializeField] public float fovRadius = 10f;
    [SerializeField] public float closeRadius = 2f;
    [SerializeField] public float attackRange = 1f;
    
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
    
    public Vector3 DestinationPos
    {
        get => KnowTargetPosition ? target.position : lastKnownTargetPosition;
    }
}
