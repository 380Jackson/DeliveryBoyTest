using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneOrbit : MonoBehaviour
{
    public GameObject Target;
    public float radius = 2.0f;
    public float orbitDegreesPerSec = 180.0f;
    public Vector3 relativeDistance = Vector3.zero;
    
    

    void Start()
    {

        if (Target != null)
        {
            relativeDistance = transform.position - Target.transform.position;
        }

    }

    private void Update()
    {
        TargetChooser();

        
    }

    void LateUpdate()
    {
        Orbit();
    }

    void Orbit()
    {
        if (Target != null)
        {
            // Keep us at the last known relative position
            transform.position = Target.transform.position + relativeDistance;
            transform.RotateAround(Target.transform.position, Vector3.up, orbitDegreesPerSec * Time.deltaTime);
            // Reset relative position after rotate
            relativeDistance = transform.position - Target.transform.position;
        }
    }

    void TargetChooser()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        foreach (RaycastHit hit in Physics.RaycastAll(ray))
        {
            if (hit.transform.gameObject.tag == "Target" || hit.transform.gameObject.tag == "Player")
            {
                if(hit.transform.gameObject.tag == "Target")
                {

                    hit.transform.gameObject.GetComponent<TargetColorBehavior>().SetTargetColor();
                }



                if (Input.GetButtonDown("Fire2"))
                {
                    Debug.Log("Hit this thing:" + hit.transform.gameObject.name);
                    Target = hit.transform.gameObject;
                    
                }
                
            }
        }
        
    }
}

