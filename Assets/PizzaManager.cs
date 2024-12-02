using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PizzaManager : MonoBehaviour
{
    // pizza health settings
    public float maxPizzaHealth = 100f;
    public float currentPizzaHealth;
    public float healthDecayRate = 2f;  // health lost per second
    public float damageFromFall = 15f;  // damage taken from hard landings
    public float fallDamageThreshold = -20f;  // velocity threshold for fall damage
    
    // scoring variables
    public static int currentScore = 0;
    public int deliveryBonus = 100;
    public int healthBonus = 50;  // extra points for delivering with high health
    
    private PlayerController playerController;
    private bool hasPizza = false;
    private Vector3 lastVelocity;
    
    // references to game objects
    public Transform[] pickupPoints;
    public Transform[] deliveryPoints;
    private Transform currentDestination;
    
    void Start()
    {
        currentScore = 0;
        playerController = GetComponent<PlayerController>();
        ResetPizzaHealth();
        SetNewDestination();
    }
    
    void Update()
    {
        if (hasPizza)
        {
            // decrease health over time
            currentPizzaHealth -= healthDecayRate * Time.deltaTime;
            
            // check fall damage
            if (playerController.velocity.y < fallDamageThreshold && 
                lastVelocity.y > playerController.velocity.y)
            {
                TakeDamage(damageFromFall);
            }
            
            // failed delivery if health reaches 0
            if (currentPizzaHealth <= 0)
            {
                FailDelivery();
            }
        }
        
        lastVelocity = playerController.velocity;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickupPoint") && !hasPizza)
        {
            PickupPizza();
        }
        else if (other.CompareTag("DeliveryPoint") && hasPizza)
        {
            CompleteDelivery();
        }
    }
    
    void PickupPizza()
    {
        hasPizza = true;
        ResetPizzaHealth();
        
        // pick random delivery point
        currentDestination = deliveryPoints[Random.Range(0, deliveryPoints.Length)];
    }
    
    void CompleteDelivery()
    {
        int totalScore = deliveryBonus;
        
        // add health-based bonus points
        totalScore += Mathf.RoundToInt((currentPizzaHealth / maxPizzaHealth) * healthBonus);
        
        currentScore += totalScore;
        hasPizza = false;
        SetNewDestination();
    }
    
    void FailDelivery()
    {
        hasPizza = false;
        SetNewDestination();
    }
    
    void SetNewDestination()
    {
        if (!hasPizza)
        {
            // pick random pickup point
            currentDestination = pickupPoints[Random.Range(0, pickupPoints.Length)];
        }
    }
    
    void ResetPizzaHealth()
    {
        currentPizzaHealth = maxPizzaHealth;
    }
    
    public void TakeDamage(float damage)
    {
        if (hasPizza)
        {
            currentPizzaHealth -= damage;
        }
    }
}
