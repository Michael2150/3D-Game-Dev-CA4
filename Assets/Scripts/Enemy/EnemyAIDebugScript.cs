using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAIMovement))]
public class EnemyAIDebugScript : MonoBehaviour
{
    [SerializeField] private EnemyAIMovement enemyAIMovement;

    private void OnDrawGizmosSelected()
    {
        try
        {
            //Draw the field of view
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, enemyAIMovement.fovRadius);

            drawFOV();
            DrawTargetLine();

            //Draw the attack range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyAIMovement.attackRange);
        }
        catch (Exception e)
        {
            return;
        }
    }

    private void drawFOV()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * enemyAIMovement.fovRadius);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(enemyAIMovement.fovAngle / 2, transform.up) * transform.forward * enemyAIMovement.fovRadius);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(-enemyAIMovement.fovAngle / 2, transform.up) * transform.forward * enemyAIMovement.fovRadius);
        Gizmos.DrawWireSphere(transform.position, enemyAIMovement.closeRadius);
    }

    private void DrawTargetLine()
    {
        enemyAIMovement.KnowTargetPosition = enemyAIMovement.TargetPosKnown();
        
        //Draw the target line
        if (enemyAIMovement.KnowTargetPosition) 
            Gizmos.color = Color.green;
        else 
            Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, enemyAIMovement.target.position);
        
        Gizmos.color = Color.magenta; ;
        Gizmos.DrawWireSphere(enemyAIMovement.DestinationPos, 0.5f);   
    }

    private void OnDrawGizmos()
    {
        try
        {
            drawFOV();
            DrawTargetLine();
        }
        catch (Exception e)
        {
            return;
        }
    }
}
