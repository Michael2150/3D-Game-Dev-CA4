using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //getter and setter for player health
    [SerializeField] private int health = 100;
    [SerializeField] private Text healthText;
    
    void Start()
    {
        Health = 100;
    }
    
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            //Make sure the health is between 0 and 100
            health = Mathf.Clamp(value, 0, 100);
            
            //Handle UI
            healthText.text = health + "%";
            
            //If the player health is 0, then the player is dead
            if (health == 0)
                PlayerDead();
        }
    }
    
    //Function to call when the player is dead
    void PlayerDead()
    {
        //Destroy the player
        Debug.LogWarning("Player is dead");
    }
}
