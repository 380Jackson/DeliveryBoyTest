using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseThing : MonoBehaviour
{
    
new public Camera camera;
public float zValue = 0;

    public enum CoordinateSystem
    {
        World,
        Screen,
        Viewport
    }
    public CoordinateSystem convertTo;
    new Transform transform;

    void Awake()
    {
        transform = GetComponent<Transform>();
    }

    void Start()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
    }

    void Update()
    {
        Vector3 converted = Input.mousePosition;

        if (convertTo == CoordinateSystem.World)
        {
            converted = camera.ScreenToWorldPoint(converted);
        }
        else if (convertTo == CoordinateSystem.Screen)
        {
            // Input.mousePosition is already in screen space
        }
        else if (convertTo == CoordinateSystem.Viewport)
        {
            converted = camera.ScreenToViewportPoint(converted);
        }

        converted.z = zValue;
        transform.position = converted;
    }

}
