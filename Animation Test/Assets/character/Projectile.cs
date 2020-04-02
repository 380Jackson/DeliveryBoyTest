using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed;
    public float fireRate;
    public float LifeTime;
    public float Damage;
    public float CurrentTime; 

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * (speed * Time.deltaTime);
        Timer();

        if (CurrentTime >= LifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    void Timer()
    {
        CurrentTime = CurrentTime + 1 * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Geo")
        {
            Destroy(this.gameObject);
        }
    }
}
