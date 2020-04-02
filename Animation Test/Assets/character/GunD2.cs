using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunD2 : MonoBehaviour
{

    private GameObject Target;
    private bool SeeTarget;
    public GameObject Bspawner;
    public GameObject Projectile;
    public GameObject Defaultlook;
    public string TargetString;
    public float FireRate;
    private float LastShot;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        SeeTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (SeeTarget == false)
        {
            // set the target to the Defualt look
            transform.LookAt(Defaultlook.transform);
        }

        
    }

    //change the target, look at the target, and fire the weapon
    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.tag == TargetString)
        {
            
            Target = other.gameObject;
            
            transform.LookAt(new Vector3(Target.transform.position.x, Target.transform.position.y + 2.5f, Target.transform.position.z));
            SeeTarget = true;
            Fire();
            
        }
        
    }
    // set the seetarget boolean as false
    private void OnTriggerExit(Collider other)
    {
        SeeTarget = false;
    }

    // fire the weapon at specific intervals 
    void Fire()
    {
        if(Time.time > FireRate + LastShot)
        {
            Instantiate(Projectile, Bspawner.transform.position, Bspawner.transform.rotation);
            LastShot = Time.time;

        }
    }

}
