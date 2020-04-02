using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Analytics;

public class SpawnObject : MonoBehaviour
{
    private AnalyticsEventTracker analyticEventTrackerComponent;

    //Initializing Variables
    public GameObject asset;
    public Transform spawnPoint;
    public ParticleSystem muzzleFlash;
    public AudioSource gunShot;
    public int ReserveAmmo;
    private int currentAmmo;
    public int maxAmmo = 10;
    public float reloadTime = 4f;
    private bool isReloading = false;
    public Transform shotText;
    public Transform ReserveText;
    private bool Shot = false;
    private bool CanShoot = true;

    // Reporting
    [HideInInspector]
    public static int shots;

    void Start()
    {
        shots = 0;

        analyticEventTrackerComponent = gameObject.GetComponent<AnalyticsEventTracker>();

        currentAmmo = maxAmmo;
    }


    void OnEnable()
    {
        currentAmmo = maxAmmo;
        ReserveAmmo = maxAmmo * 3;
        shotText.GetComponent<Text>().text = currentAmmo.ToString();
        isReloading = false;

    }
    // Use this for initialization
    void Update()
    {
        ReserveText.GetComponent<Text>().text = ReserveAmmo.ToString();



        if (currentAmmo == 0)
        {

            if (ReserveAmmo > 1)
            {

                isReloading = true;
                StartCoroutine(Reload());

            }

        }
        //Instantiate Game Object
        if (Input.GetButtonDown("Fire1") || Input.GetAxis("Rtrigger") > 0 && Shot == false)
        {
            if (isReloading == false)
            {
                shots++;
                Fire();
            }
        }

        if (Input.GetAxis("Rtrigger") < 1)
        {
            Shot = false;
        }

        IEnumerator Reload()
        {

            Debug.Log("Reloading");
            shotText.GetComponent<Text>().text = ("Reloading");
            if (ReserveAmmo <= maxAmmo)
            {
                currentAmmo = ReserveAmmo;
                ReserveAmmo = 0;
            }

            if (ReserveAmmo > maxAmmo)
            {
                currentAmmo = maxAmmo;
                ReserveAmmo -= maxAmmo;
            }
            yield return new WaitForSeconds(reloadTime);



            isReloading = false;
            shotText.GetComponent<Text>().text = currentAmmo.ToString();
        }

        if (Health.isPlayerDead == true)
        {

        }

    }
    void Fire()
    {
        gunShot.Play();
        muzzleFlash.Play();
        GameObject bullet = Instantiate(asset, spawnPoint.position, spawnPoint.rotation);
        currentAmmo--;
        shotText.GetComponent<Text>().text = currentAmmo.ToString();
        Shot = true;
    }

    private void ReportShotsFired()
    {
        analyticEventTrackerComponent.TriggerEvent();
        shots = 0;
    }

}
