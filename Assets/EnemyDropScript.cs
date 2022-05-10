using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropScript : MonoBehaviour
{
    private EnemyAI enemyAI;
    [SerializeField] private GameObject ammoDropPrefab;
    [SerializeField] private GameObject healthDropPrefab;
    [SerializeField] private float ammoDropChance = 0.5f;
    [SerializeField] private float healthDropChance = 0.3f;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyAI = GetComponent<EnemyAI>();    
    }

    // Update is called once per frame
    void Update()
    {
        //The moment the enemy dies, drop an item
        if (enemyAI.State == EnemyAI.EnemyState.Dead)
        {
            //Randomly decide if an item will drop
            float dropChance = Random.Range(0f, 1f);
            if (dropChance <= ammoDropChance)
            {
                //Drop an ammo drop and apply a force to it
                GameObject ammoDrop = Instantiate(ammoDropPrefab, transform.position, Quaternion.identity);
                ammoDrop.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(0f, 1f), ForceMode.Impulse);
            }

            if (dropChance <= healthDropChance)
            {
                //Drop an ammo drop and apply a small random force to it
                GameObject healthDrop = Instantiate(healthDropPrefab, transform.position, Quaternion.identity);
                healthDrop.GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(0f, 1f), ForceMode.Impulse);
            }
            
            //If the enemy has a capsule collider, destroy it
            if (GetComponent<CapsuleCollider>())
                GetComponent<CapsuleCollider>().enabled = false;
            
            //Disable this script
            enabled = false;
        }
    }
}
