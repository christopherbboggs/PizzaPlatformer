using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 movement;
    public float speed = 10;
    public float gravity = 15;
    public float jump = 4;

    public float rotationSpeed = 90;
    private float rotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (controller.isGrounded)
        {
            movement = new Vector3(horizontalInput, 0,0 );
            movement = this.transform.TransformDirection(movement);

            if (Input.GetButton("Jump"))
            {
                movement.y = jump;
            }
        }
       
        this.transform.eulerAngles = new Vector3(0, rotation, 0);

        movement.y -= gravity * Time.deltaTime;
        controller.Move(movement * speed * Time.deltaTime);
    }
}