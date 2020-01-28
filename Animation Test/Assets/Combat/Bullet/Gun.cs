using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    private GameObject Target;
    private bool SeeTarget;
    public GameObject Bspawner;
    public GameObject Projectile;

    // Start is called before the first frame update
    void Start()
    {
        SeeTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.tag == "Target")
        {
            
            Target = other.gameObject;
            transform.LookAt(Target.transform);
            SeeTarget = true;
            if (Input.GetButton("DroneFire"))
            {
                Instantiate(Projectile, Bspawner.transform.position, Bspawner.transform.rotation);
            }
        }
        
    }


    
}
