using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetColorBehavior : MonoBehaviour
{
    public Color DefualtColor;
    public Color TargetSelect;
    public bool Targeted;
    // Start is called before the first frame update
    void Start()
    {

        Targeted = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Targeted == false)
        {
            GetComponent<Renderer>().material.SetColor("_BaseColor", DefualtColor);
        }
        if (Targeted == true)
        {
            GetComponent<Renderer>().material.SetColor("_BaseColor", TargetSelect);
        }

        Targeted = false;
    }

    public void SetTargetColor()
    {
        Targeted = true;
    }
}
