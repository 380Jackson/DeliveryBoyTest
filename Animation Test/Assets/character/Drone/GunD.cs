using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunD : MonoBehaviour
{

    private GameObject ShootTarget;
    private bool SeeTarget;
    public GameObject Bspawner;
    public GameObject Projectile;
    public GameObject Defaultlook;
    public float PsensorLength;
    public float FireRate;
    private float LastShot;
    public DroneOrbit Dr;
    
    
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

        if (Dr.Target.tag == "Target")
        {

            ShootTarget = Dr.Target.gameObject;

            transform.LookAt(new Vector3(ShootTarget.transform.position.x, ShootTarget.transform.position.y + 2.5f, ShootTarget.transform.position.z));
            SeeTarget = true;
            Fire();

        }

        if (Dr.Target.tag != "Target")
        {
            SeeTarget = false;
        }
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
