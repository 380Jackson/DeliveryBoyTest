using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    private GameObject Target;
    private bool SeeTarget;
    public GameObject Bspawner;
    public GameObject Projectile;
    public GameObject Defaultlook;
    public float PsensorLength;
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
            transform.LookAt(Defaultlook.transform);
        }

        
    }


    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.tag == "Target")
        {
            
            Target = other.gameObject;
            
            transform.LookAt(new Vector3(Target.transform.position.x, Target.transform.position.y + 2.5f, Target.transform.position.z));
            SeeTarget = true;
            Fire();
            
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        SeeTarget = false;
    }

    void Fire()
    {
        if(Time.time > FireRate + LastShot)
        {
            Instantiate(Projectile, Bspawner.transform.position, Bspawner.transform.rotation);
            LastShot = Time.time;

        }
    }

}
