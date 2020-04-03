using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Health healthComponent;

    [SerializeField]
    private int id = 0;

    [SerializeField]
    private Transform visual;

    /// <summary>
    /// The movement component attached to this player.
    /// </summary>
    public PlayerMovementV2 Movement { get; private set; }

    private Vector3 lookPos;
    /// <summary>
    /// Can the player look around using the left stick?
    /// </summary>
    public bool CanLookAround { get; set; } = true;

    public bool CanMove { get; set; } = true;
    /// <summary>
    /// Rotation that the player should be looking towards based on left stick.
    /// </summary>
    /// 

    // Lava parameters
    private bool isTouchingLava;
    [SerializeField]
    private float timeBetweenLavaDamage;
    private float elapsingTimeBetweenLavaDamage;

    public float GetRotation(Vector3 stick)
    {
        float x = stick.x;
        float y = stick.y;
        Vector3 lookDir = new Vector3(x, y).normalized;
        return Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg;
    }

    public Transform Visual => visual;

    private void Awake()
    {
        healthComponent = gameObject.GetComponent<Health>();
        Movement = GetComponent<PlayerMovementV2>();
        OnAwake();

        elapsingTimeBetweenLavaDamage = timeBetweenLavaDamage;
    }

    protected virtual void OnAwake()
    {

    }

    public Gamepad Gamepad
    {
        get
        {
            if (id >= 0 && id < Gamepad.all.Count)
            {
                return Gamepad.all[id];
            }

            return null;
        }
    }

    /// <summary>
    /// Value from 0 to 1 from the left trigger, or right trigger, or shift on the keyboard.
    /// </summary>
    public float Trigger
    {
        get
        {
            float kb = Keyboard.current.leftShiftKey.isPressed ? 1 : 0;
            float right = Gamepad?.rightTrigger?.ReadValue() ?? default;
            float left = Gamepad?.leftTrigger?.ReadValue() ?? default;
            return Mathf.Max(kb, right, left);
        }
    }

    /// <summary>
    /// A vector that represents the left stick on a gamepad, or a WASD.
    /// </summary>
    public Vector2 LeftStick
    {
        get
        {
            Gamepad gamepad = Gamepad;
            if (gamepad != null)
            {
                return gamepad.leftStick.ReadValue().normalized;
            }
            else
            {
                Vector2 vector = default;
                if (Keyboard.current.wKey.isPressed)
                {
                    vector.y += 1;
                }
                if (Keyboard.current.aKey.isPressed)
                {
                    vector.x -= 1;
                }
                if (Keyboard.current.sKey.isPressed)
                {
                    vector.y -= 1;
                }
                if (Keyboard.current.dKey.isPressed)
                {
                    vector.x += 1;
                }

                return vector.normalized;
            }
        }
    }

    /// <summary>
    /// A vector that represents the right stick on a gamepad, or arrow keys.
    /// </summary>
    public Vector2 RightStick
    {
        get
        {
            Gamepad gamepad = Gamepad;
            if (gamepad != null)
            {
                return gamepad.rightStick.ReadValue().normalized;
            }
            else
            {
                RaycastHit _hit;
                Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(_ray, out _hit))
                {
                    lookPos = _hit.point;
                }

                Vector3 lookDir = lookPos - transform.position;

                lookDir.y = 0;

                transform.LookAt(transform.position + lookDir, Vector3.up);
                //Vector2 Newlook = new Vector2(lookPos.x,lookPos.z);

                return default;
            }
        }
    }

    public int ControllerIndex
    {
        get => id;
        set => id = value;
    }

    private void Update()
    {
        //send inputs to the movement thingy
        float x = LeftStick.x;
        float y = LeftStick.y;
        bool jump = Gamepad?.buttonSouth?.isPressed ?? Keyboard.current.spaceKey.isPressed;
        Vector2 input = new Vector2(x, y);
        Movement.Input = CanMove ? input : default;
        Movement.Jump = CanMove ? jump : default;

        if (CanLookAround)
        {
            LookAround(RightStick);
        }

        OnUpdate();

        //check if player is on screen
        Vector2 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPoint.x < -0.1f || screenPoint.x > 1.1f || screenPoint.y < -0.1f || screenPoint.y > 1.2f || transform.position.y < -2f)
        {
            //is oob
            transform.position = new Vector3(0f, 8f, 0f);
            Movement.Rigidbody.velocity = Vector3.zero;
        }

        if (isTouchingLava == true)
        {
            elapsingTimeBetweenLavaDamage -= Time.deltaTime;

            if (elapsingTimeBetweenLavaDamage <= 0.0f)
            {
                healthComponent.health--;
                elapsingTimeBetweenLavaDamage = timeBetweenLavaDamage;
            }


        }
    }

    protected virtual void OnUpdate()
    {

    }

    public void LookAround(Vector3 stick)
    {
        //looking around
        float x = stick.x;
        float y = stick.y;

        //check if joystick looked far enough
        Vector3 lookDir = new Vector3(x, y);
        if (lookDir.sqrMagnitude > 0.25f)
        {
            //normalize the thingy
            lookDir.Normalize();

            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.y = Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = eulerAngles;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Lava")
        {
            isTouchingLava = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Lava")
        {
            isTouchingLava = false;
            elapsingTimeBetweenLavaDamage = timeBetweenLavaDamage;
        }
    }
}
