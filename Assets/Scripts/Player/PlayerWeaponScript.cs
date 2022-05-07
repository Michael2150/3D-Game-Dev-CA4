using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponScript : MonoBehaviour, IHitter
{
    //Ammo
    [SerializeField] private int maxAmmo = 150;
    [SerializeField] private int reserveAmmo = 30;
    [SerializeField] private int maxClip = 30;
    [SerializeField] private int currentClip = 30;
    [SerializeField] private float reloadTime = 1.5f;
    [SerializeField] private float fireRate = 10f; //Rounds per second
    [SerializeField] private float hitDamage = 10f;
    
    [SerializeField] private ParticleSystem muzzleFlashEffect;
    [SerializeField] private List<AudioSource> gunSounds;
    [SerializeField] private GameObject EnemyHitEffect;
    [SerializeField] private GameObject ObjectHitEffect;
    
    [SerializeField] private Text ammoText;
    [SerializeField] private Image reloadingBar;
    [SerializeField] private Text crosshair;
    
    [SerializeField] private Camera fpsCamera;
    
    [SerializeField] private MouseLook mouseLook;

    public bool isShooting = false;
    private bool isReloading = false;

    private float nextFire = 0f;
    
    private void Start()
    {
        CurrentClip = maxClip;
    }

    private void Update()
    {
        if (isShooting)
        {
            if (CurrentClip > 0)
            {
                //Make sure the fire rate is not exceeded
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + 1 / fireRate;
                    Shoot();
                }
            }
            else if (!isReloading)
            {
                Reload();
            }
        } 
    }
    
    //Shoot method
    public void Shoot()
    {
        //Play random muzzle flash on the position of the muzzle position and rotation of the muzzle position if there is any
        if (muzzleFlashEffect)
        {
            muzzleFlashEffect.Play();
        }


        //Play random gun sound if there is any
        if (gunSounds.Count > 0)
            gunSounds[Random.Range(0, gunSounds.Count)].Play();
        
        //Handle the hitscan raycast
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, 100f))
        {
            Debug.Log("Hit: " + hit.transform.name);
            
            //See if the object has a IHittable interface
            IHittable hitObject = hit.transform.GetComponent<IHittable>();
            if (hitObject != null)
                hitObject.Hit(gameObject, this);
            
            //Spawn the hit effect
            GameObject hitEffect = Instantiate((hit.transform.tag == "Enemy") ? EnemyHitEffect : ObjectHitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Make the hit effect child of the hit object
            hitEffect.transform.parent = hit.transform;
            //Destroy the hit effect after 1 second
            Destroy(hitEffect, 1f);
        }
        
        //Handle Ammo
        CurrentClip--;
    }
    
    //current clip getter and setter
    private int CurrentClip
    {
        get { return currentClip; }
        set
        {
            currentClip = value;
            ammoText.text = currentClip + " / " + reserveAmmo;
        }
    }
    
    public int ReserveAmmo
    {
        get { return reserveAmmo; }
        set
        {
            //clamp between 0 and maxAmmo
            reserveAmmo = Mathf.Clamp(value, 0, maxAmmo);
            ammoText.text = currentClip + " / " + reserveAmmo;
        }
    }
    
    //Reloading
    public void Reload()
    {
        if (currentClip < maxClip)
        {
            //if there is ammo in the reserve
            if (reserveAmmo > 0)
                StartCoroutine(Reloading());
        } 
    }
    
    //Reloading coroutine
    private IEnumerator Reloading()
    {
        isReloading = true;
        
        //Handle visual
        reloadingBar.gameObject.SetActive(true);
        crosshair.gameObject.SetActive(false);
        float reloadingBarFill = 0;
        while (reloadingBarFill < 1)
        {
            reloadingBarFill += Time.deltaTime / reloadTime;
            reloadingBar.fillAmount = reloadingBarFill;
            yield return null;
        }
        reloadingBar.gameObject.SetActive(false);
        crosshair.gameObject.SetActive(true);
        
        //Handle ammo logic
        int ammoToReload = maxClip - currentClip;
        if (reserveAmmo >= ammoToReload)
        {
            reserveAmmo -= ammoToReload;
            CurrentClip = maxClip;
        }
        else
        {
            var curReserveAmmo = reserveAmmo;
            reserveAmmo = 0;
            CurrentClip += curReserveAmmo;
        }
        
        isReloading = false;
    }

    public float getHitDamage()
    {
        return hitDamage;
    }
}
