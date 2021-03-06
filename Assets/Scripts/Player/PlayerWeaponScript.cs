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
    [SerializeField] private List<AudioClip> gunSounds;
    [SerializeField] private AudioSource audioSource;
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
        fillPools();
        
        audioSource = GetComponent<AudioSource>();
    }

    //Pooling
    private List<GameObject> enemyHitPool;
    private List<GameObject> objectHitPool;
    private GameObject poolObject;
    private void fillPools()
    {
        //Create the pool gameobject
        poolObject = new GameObject("Pool");
        poolObject.transform.SetParent(GameManager.Instance.transform);
        
        //Fills the pool of enemy hit effects
        enemyHitPool = new List<GameObject>();
        objectHitPool = new List<GameObject>();

        for (int i = 0; i < 20; i++)
        {
            if (EnemyHitEffect != null)
            {
                GameObject newEnemyHit = Instantiate(EnemyHitEffect, poolObject.transform, true);
                newEnemyHit.SetActive(false);
                enemyHitPool.Add(newEnemyHit);       
            }

            if (ObjectHitEffect != null)
            {
                GameObject newObjectHit = Instantiate(ObjectHitEffect, poolObject.transform, true);
                newObjectHit.SetActive(false);
                objectHitPool.Add(newObjectHit);   
            }
        }
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
        {
            //Play a random gun sound
            audioSource.clip = gunSounds[Random.Range(0, gunSounds.Count)];
            //Random pitch
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            //Play the sound
            audioSource.Play();
        }

        //Handle the hitscan raycast
        RaycastHit hit;
        if (Physics.Raycast(fpsCamera.transform.position, fpsCamera.transform.forward, out hit, 100f))
        {
            //See if the object has a IHittable interface
            IHittable hitObject = hit.transform.GetComponent<IHittable>();
            if (hitObject != null)
                hitObject.Hit(gameObject, this);
            
            //Grab a hit effect from the the enemy hit pool if it is an enemy or from the object hit pool if it is an object
            GameObject hitEffect = null;
            if (hit.transform.tag == "Enemy")
            {
                if (enemyHitPool.Count > 0)
                {
                    hitEffect = enemyHitPool[0];
                    enemyHitPool.RemoveAt(0);
                }
            }
            else
            {
                if (objectHitPool.Count > 0)
                {
                    hitEffect = objectHitPool[0];
                    objectHitPool.RemoveAt(0);
                }
            }
            
            //If there is a hit effect, play it on the position of the hit and rotation of the hit
            if (hitEffect != null)
            {
                hitEffect.transform.position = hit.point;
                hitEffect.transform.rotation = Quaternion.LookRotation(hit.normal);
                hitEffect.SetActive(true);

                //Return the hit effect to the pool after it has played
                StartCoroutine(ReturnHitEffect(hitEffect, hit));
            }
        }
        
        //Handle Ammo
        CurrentClip--;
    }

    private IEnumerator ReturnHitEffect(GameObject hitEffect, RaycastHit hit)
    {
        yield return new WaitForSeconds(0.5f);
        hitEffect.SetActive(false);
        if (hit.transform.tag == "Enemy")
            enemyHitPool.Add(hitEffect);
        else
            objectHitPool.Add(hitEffect);
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
