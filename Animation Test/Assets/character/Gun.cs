using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{


    public GameObject Bspawner;
    public GameObject Projectile;

    
    public float FireRate;
    private float LastShot;
    
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButtonDown("Fire1"))
        {

            Instantiate(Projectile, Bspawner.transform.position, Bspawner.transform.rotation);
        }
        
    }
    

    

}
