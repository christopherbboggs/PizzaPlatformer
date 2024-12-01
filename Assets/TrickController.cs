using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrickController : MonoBehaviour
{
    private PlayerController playerController;
    private CharacterController controller;
    public enum TrickState {IDLE, BACKFLIP, FRONTFLIP, SPIN_LEFT, SPIN_RIGHT};
    public TrickState trickState;
    public List<TrickState> completedTricks;
    private bool doTrick = false;
    private bool boost = false;
    public Quaternion startRotation;
    public float trickTimer = 0;
    public float boostTimer = 0;
    public float boostLength = 3f;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        controller = GetComponent<CharacterController>();
        completedTricks = new List<TrickState>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.isGrounded && completedTricks.Count > 0) // When we land after completing any number of tricks, grant speed & acceleration boosts for a limited durattion.
        {

            Debug.Log("Trick chain completed: " + string.Join(", ", completedTricks));
            if (boost)
            {
                if (1 * completedTricks.Count > playerController.accBoost) // If we were already in boost, only reset the timer and change the speed & acceleration boosts if the new trick chain was greater than the previous one.
                {
                    boostTimer = boostLength;
                    playerController.accBoost = 1 * completedTricks.Count;
                    playerController.maxSpeedBoost = 5 * completedTricks.Count;
                }
            }
            else
            {
                boost = true;
                boostTimer = boostLength;
                playerController.accBoost = 1 * completedTricks.Count;
                playerController.maxSpeedBoost = 5 * completedTricks.Count;
            }

            // Boost current velocity when we land a trick chain regardless.
            Vector3 vel = playerController.velocity;
            if (vel.x > 0) vel.x += 5 * completedTricks.Count;
            else if (vel.x < 0) vel.x -= 5 * completedTricks.Count;
            playerController.velocity = vel;

            completedTricks.Clear(); // Wipe out the trick list after granting the boosts.
        }
        if (boost && boostTimer > 0) // Count down timer
        {
            boostTimer -= Time.deltaTime;
        }
        if (boost && boostTimer <= 0) // End boost and revert acceleration and speed boosts.
        {
            boostTimer = 0;
            boost = false;
            playerController.accBoost = 0;
            playerController.maxSpeedBoost = 0;
        }
    }
    void LateUpdate()
    {
        // Handle input for tricks while in the air and not already performing another trick
        if (!controller.isGrounded && !doTrick && Input.GetKeyDown(KeyCode.Q))
        {
            startRotation = transform.rotation;
            doTrick = true;
            trickState = TrickState.BACKFLIP;
            
        }
        if (!controller.isGrounded && !doTrick && Input.GetKeyDown(KeyCode.E))
        {
            startRotation = transform.rotation;
            doTrick = true;
            trickState = TrickState.FRONTFLIP;
            
        }
        if (!controller.isGrounded && !doTrick && Input.GetKeyDown(KeyCode.W))
        {
            startRotation = transform.rotation;
            doTrick = true;
            trickState = TrickState.SPIN_LEFT;
            
        }
        if (!controller.isGrounded && !doTrick && Input.GetKeyDown(KeyCode.S))
        {
            startRotation = transform.rotation;
            doTrick = true;
            trickState = TrickState.SPIN_RIGHT;
            
        }
        else if (doTrick && trickTimer < 0.5f) // Handle rotation of player based on trick inputted.
        {
            trickTimer += Time.deltaTime;

            switch (trickState)
            {
                case TrickState.BACKFLIP:
                    transform.RotateAround(transform.position, Vector3.forward, 720 * Time.deltaTime);
                    break;
                case TrickState.FRONTFLIP:
                    transform.RotateAround(transform.position, Vector3.forward, -720 * Time.deltaTime);
                    break;
                case TrickState.SPIN_LEFT:
                    transform.RotateAround(transform.position, Vector3.up, -720 * Time.deltaTime);
                    break;
                case TrickState.SPIN_RIGHT:
                    transform.RotateAround(transform.position, Vector3.up, 720 * Time.deltaTime);
                    break;
            }
        }
        else if (doTrick && trickTimer >= 0.4f) // When a trick successfully completes, add it to the list and reset rotation and trick state.
        {
            completedTricks.Add(trickState);
            transform.rotation = startRotation;
            trickState = TrickState.IDLE;
            trickTimer = 0;
            doTrick = false;
        }
        if (doTrick && controller.isGrounded) // If we land while in trick state, wipe out the trick list, reset our trick state and rotation, and lose some speed.
        {
            Debug.Log("Trick failed! Trick type: " + trickState);
            completedTricks.Clear();
            playerController.velocity *= 0.5f;
            transform.rotation = startRotation;
            trickState = TrickState.IDLE;
            trickTimer = 0;
            doTrick = false;
        }
    }
}
