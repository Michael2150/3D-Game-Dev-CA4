using UnityEngine;

public class AmmoDropScript : MonoBehaviour, IInteractable
{
    [SerializeField] private int ammoToAdd = 10;

    private void Start()
    {
        //Set the ammo to add to a random number between 10 and 30
        ammoToAdd = Random.Range(10, 30);
    }

    public void Interact(GameObject sender)
    {
        //Go through all the children in the sender recursively and find the PlayerWeaponScript
        PlayerWeaponScript playerWeaponScript = sender.GetComponentInChildren<PlayerWeaponScript>();
        if (playerWeaponScript)
            playerWeaponScript.ReserveAmmo += ammoToAdd;
        
        //Destroy the ammo drop
        Destroy(gameObject);
    }

    public string getInteractableText()
    {
        return $"Press E to pick up {ammoToAdd} ammo";
    }
}
