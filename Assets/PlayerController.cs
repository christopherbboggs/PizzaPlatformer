using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private TrickController trickController;
    private float horizontalInput;
    private float verticalInput;
    public Vector3 velocity;
    public float acceleration = 15;
    public float accBoost = 0;
    public float maxSpeed = 40;
    public float maxSpeedBoost = 0;
    public float gravity = 30;
    public float jumpHeight = 15;
    public float fallMult = 2.5f;
    public float lowJumpMult = 2f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        trickController = GetComponent<TrickController>();
        velocity = new Vector3(0, 0, 0);
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        float accMult = horizontalInput;

        // Clip speed to 0 at small values to keep the player from jittering in place.
        if (Mathf.Abs(velocity.x) < 0.025f) velocity.x = 0;
        if (Mathf.Abs(velocity.y) < 0.025f) velocity.y = 0;

        if (controller.isGrounded)
        {
            if (Input.GetButton("Jump"))
            {
                velocity.y += Mathf.Sqrt(jumpHeight * 2.0f * gravity);
            }
        }
        else
        {
            accMult *= 0.6f; // Player should have less control over horizontal velocity in the air then when grounded.
        }
        if (horizontalInput == 0 && velocity.x != 0) accMult = (velocity.x > 0) ? -0.4f : accMult = 0.4f; // Slowly decelerate if no input is being held while moving.
        if ((horizontalInput > 0 && velocity.x < 0) || (horizontalInput < 0 && velocity.x > 0)) accMult *= 6; // Multiply braking force if input is in opposite direction of current velocity.

        velocity.x += accMult * (acceleration + accBoost) * Time.deltaTime;

        // Cap our speed if it is over the max speed in either direction;
        if (velocity.x > (maxSpeed + maxSpeedBoost)) velocity.x = (maxSpeed + maxSpeedBoost);
        else if (velocity.x < -(maxSpeed + maxSpeedBoost)) velocity.x = -(maxSpeed + maxSpeedBoost);

        if(trickController.trickState == TrickController.TrickState.IDLE)
        {
            Quaternion rotation = transform.rotation;
            rotation.y = (velocity.x >= 0) ? 0 : 180;
            transform.rotation = rotation;
        }

        if (velocity.y < 0) velocity.y -= gravity * fallMult * Time.deltaTime; // Increase the effect of gravity while falling for snappier movement.
        else if (velocity.y > 0 && !Input.GetButton("Jump")) velocity.y -= gravity * lowJumpMult * Time.deltaTime; // Increase the effect of gravity if we release the jump button early to allow for 'short-hopping'.
        else velocity.y -= gravity * Time.deltaTime;

        velocity.z = 0; // Safety measure to prevent the player from skewing off in the z-axis due to any wackiness with 3D physics calculations.
        controller.Move(velocity * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.DrawRay(hit.point, hit.normal, Color.green, 2, false);
        if (hit.gameObject.tag == "Grindrail")
        {
            if (velocity.x > 0)
                velocity.x = Mathf.Max(velocity.x, (2/3f) * (maxSpeed + maxSpeedBoost));
            else if (velocity.x < 0)
                velocity.x = Mathf.Min(velocity.x, -(2 / 3f) * (maxSpeed + maxSpeedBoost));
            velocity = Vector3.ProjectOnPlane(velocity, hit.normal);
        }
        else
        {
            velocity -= (hit.normal * Vector3.Dot(velocity, hit.normal)); // calculate normal force to apply back to the player after collision.
        }
    }
}

    