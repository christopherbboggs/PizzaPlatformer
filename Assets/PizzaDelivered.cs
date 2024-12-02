using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PizzaDelivered : MonoBehaviour
{
    Text deliveredText;
    // Start is called before the first frame update
    void Start()
    {
        GameObject deliveredTextGO = GameObject.Find("PizzaDelivered");
        deliveredText = deliveredTextGO.GetComponent<Text>();
        if (TimeController.timeLeft > 0)
            deliveredText.text = "Pizza delivered in time!";
        else
            deliveredText.text = "The pizza went bad...";
    }
}
