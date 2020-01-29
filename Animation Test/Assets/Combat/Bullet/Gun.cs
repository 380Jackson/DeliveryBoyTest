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
    public int FireRate;

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
            transform.LookAt(Target.transform);
            SeeTarget = true;
            StartCoroutine(Fire());
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        SeeTarget = false;
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(FireRate);
        Instantiate(Projectile, Bspawner.transform.position, Bspawner.transform.rotation);
    }

}
