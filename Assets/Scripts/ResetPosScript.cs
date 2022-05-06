using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosScript : MonoBehaviour
{
    private Vector3 startPos;
    private Quaternion startRot;
    
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.localPosition;
        startRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = startPos;
        transform.localRotation = startRot;
        
        //Disable the script when the death animation is playing
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dying"))
            this.enabled = false;
    }
}
