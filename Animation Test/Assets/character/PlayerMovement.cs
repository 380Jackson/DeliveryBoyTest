using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    public Animator animator;
    private GameObject model;
    private void Start()
    {
        //Animator animator = GetComponent<Animator>();
        model = this.gameObject;
    }

    private void Update()
    {
        /* START LOCOMOTION */

        // Get the axis from the left stick (a Vector2 with the left stick's direction)
        Vector2 leftStickInputAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector2 lastLeftStickInputAxis = leftStickInputAxis;
        // Get the angle between the the direction the model is facing and the input axis vector
        var a = SignedAngle(new Vector3(leftStickInputAxis.x, 0, leftStickInputAxis.y), model.transform.forward);

        // Normalize the angle
        if (a < 0)
        {
            a *= -1;
        }
        else
        {
            a = 360 - a;
        }

        // Take into consideration the angle of the camera
        a += Camera.main.transform.eulerAngles.y;

        var aRad = Mathf.Deg2Rad * a; // degrees to radians

        // If there is some form of input, calculate the new axis relative to the rotation of the model
        if (leftStickInputAxis.x != 0 || leftStickInputAxis.y != 0)
        {
            leftStickInputAxis = new Vector2(Mathf.Sin(aRad), Mathf.Cos(aRad));
        }

        float xVelocity = 0f, yVelocity = 0f;
        float smoothTime = 0.05f;

        // Interpolate between the input axis from the last frame and the new input axis we calculated
        leftStickInputAxis = new Vector2(Mathf.SmoothDamp(lastLeftStickInputAxis.x, leftStickInputAxis.x, ref xVelocity, smoothTime), Mathf.SmoothDamp(lastLeftStickInputAxis.y, leftStickInputAxis.y, ref yVelocity, smoothTime));

        // Update the Animator with our values so that the blend tree updates
        animator.SetFloat("VelX", leftStickInputAxis.x);
        animator.SetFloat("VelZ", leftStickInputAxis.y);

        

        /* END LOCOMOTION */



        /* START ROTATION */

        // Get the axis from the right stick (a Vector2 with the right stick's direction)
        var rightStickInputAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (rightStickInputAxis.x != 0 || rightStickInputAxis.y != 0)
        {
            float angle2 = 0;
            if (rightStickInputAxis.x != 0 || rightStickInputAxis.y != 0)
            {
                angle2 = Mathf.Atan2(rightStickInputAxis.x, rightStickInputAxis.y) * Mathf.Rad2Deg;
                if (angle2 < 0)
                {
                    angle2 = 360 + angle2;
                }
            }

            // Calculate the new rotation for the model and apply it
            var rotationTo = Quaternion.Euler(0, angle2 + Camera.main.transform.eulerAngles.y, 0);
            model.transform.rotation = Quaternion.Slerp(model.transform.rotation, rotationTo, Time.deltaTime * 10);
        }

        /* END ROTATION */
    }

    private float SignedAngle(Vector3 a, Vector3 b)
    {
        return Vector3.Angle(a, b) * Mathf.Sign(Vector3.Cross(a, b).y);
    }
}
