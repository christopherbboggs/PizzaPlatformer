using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 movement;
    private float rotation;
    public float acceleration = 10;
    public float speed = 0;
    public float maxSpeed = 30;
    public float gravity = 30;
    public float jumpHeight = 10;
    public float fallMult = 1.5f;
    public float lowJumpMult = 1.5f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        movement = new Vector3(0, 0, 0);
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float accMult = horizontalInput;

        if (controller.isGrounded)
        {
            if (movement.y < 0) movement.y = 0f;
            if (Input.GetButton("Jump"))
            {
                movement.y += Mathf.Sqrt(jumpHeight * 2.0f * gravity);
            }
        }
        else
        {
            accMult *= 0.6f; // Player should have slightly less control in the air then when grounded.
        }
        if (horizontalInput == 0) accMult = (speed > 0) ? -0.4f : accMult = 0.4f; // Slowly decelerate if no input is being held.
        if ((horizontalInput > 0 && speed < 0) || (horizontalInput < 0 && speed > 0)) accMult *= 2; // Multiply braking force if input is in opposite direction of current movement.

        speed += accMult * acceleration * Time.deltaTime;

        // Clip speed to 0 at small values to keep the player from jittering in place.
        if (Mathf.Abs(speed) < 0.025f) speed = 0;
        // Cap our speed if it is over the max speed in either direction;
        if (speed > maxSpeed) speed = maxSpeed;
        else if (speed < -maxSpeed) speed = -maxSpeed;

        movement.x = speed;
        movement = this.transform.TransformDirection(movement);

        this.transform.eulerAngles = new Vector3(0, rotation, 0);

        if (movement.y < 0) movement.y -= gravity * fallMult * Time.deltaTime;
        else if (movement.y > 0 && !Input.GetButton("Jump")) movement.y -= gravity * lowJumpMult * Time.deltaTime;
        else movement.y -= gravity * Time.deltaTime;

        movement.z = 0; // Prevent the player from skewing off in the z-axis due to 3D physics calculations.

        controller.Move(movement * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.DrawRay(hit.point, hit.normal, Color.green, 2, false);
        Vector3 normalForce = (hit.normal * Vector3.Dot(movement, hit.normal));
        speed -= normalForce.x;
        movement.y -= normalForce.y;
    }
}

    