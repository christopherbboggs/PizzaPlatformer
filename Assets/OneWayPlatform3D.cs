using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform3D : MonoBehaviour
{
    public LayerMask playerLayer; // The layer the player is assigned to (e.g., "Player")
    private BoxCollider platformCollider; // Reference to the platform's BoxCollider

    private void Start()
    {
        platformCollider = GetComponent<BoxCollider>(); // Get the platform's BoxCollider
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only affect player objects
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null && playerRigidbody.velocity.y < 0)
            {
                // If the player is falling, ignore collision (go through the platform)
                Physics.IgnoreCollision(other, platformCollider, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Restore collision when the player stops falling or exits the platform's trigger
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // Only restore collision when player is no longer falling
                if (playerRigidbody.velocity.y >= 0)
                {
                    Physics.IgnoreCollision(other, platformCollider, false);
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When the player lands on the platform, restore collision
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            Rigidbody playerRigidbody = collision.collider.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // Check if player is falling and should go through, or if they're on top of the platform
                if (playerRigidbody.velocity.y >= 0)
                {
                    // If the player is not falling or is moving upward, restore collision
                    Physics.IgnoreCollision(collision.collider, platformCollider, false);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // When the player leaves the platform area, restore collision
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            Physics.IgnoreCollision(collision.collider, platformCollider, false);
        }
    }
}
