using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    private Transform targetLastKnownPosition;
    [SerializeField] private float fovAngle = 90f;
    [SerializeField] private float fovRadius = 10f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackRate = 1f;
    
    
    // Update is called once per frame
    void Update()
    {
        updateDestination();
        updateAngle();
    }

    void updateDestination()
    {
        //If the player is in the field of view, move towards the player. Otherwise, move towards the last known position of the player.
        agent.SetDestination(isTargetInFOV() ? target.position : targetLastKnownPosition.position);
    }
    
    bool isTargetInFOV()
    {
        //Check if the target is in the field of view.
        Vector3 targetDirection = target.position - transform.position;
        float targetAngle = Vector3.Angle(targetDirection, transform.forward);
        bool isInFOV = targetAngle <= fovAngle/2;
        
        //If the target is in the field of view, update the last known position of the target.
        if (isInFOV)
        {
            targetLastKnownPosition = null;
        } else {
            if (targetLastKnownPosition == null)
                targetLastKnownPosition = target;
        }
        
        return isInFOV;
    }

    void updateAngle()
    {
        
    }

    
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        
        //Draw the field of view
        Gizmos.DrawWireSphere(transform.position, fovRadius);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * fovRadius);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(fovAngle / 2, transform.up) * transform.forward * fovRadius);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(-fovAngle / 2, transform.up) * transform.forward * fovRadius);
        
        //Draw the attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        //Draw the target line
        if (isTargetInFOV()) 
            Gizmos.color = Color.green;
        else 
            Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, target.position);

        //Draw a blue sphere at the target's last known position.
        if (targetLastKnownPosition)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(targetLastKnownPosition.position, 0.5f);    
        }
    }
}
