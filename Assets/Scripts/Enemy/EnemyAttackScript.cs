using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour, IHitter
{
    [SerializeField] private float attackDamage = 30f;
    
    public float getHitDamage()
    {
        return attackDamage;
    }

    public void Attack(float AttackRadius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, AttackRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            //Check if the collider's gameobject has a IHittable component
            IHittable hitObject = hitCollider.GetComponent<IHittable>();
            if (hitObject != null)
                hitObject.Hit(gameObject, this);
        }
    }
}
