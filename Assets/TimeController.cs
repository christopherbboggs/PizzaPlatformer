using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{

    Text timeText;
    float startingTime;

    
    // Start is called before the first frame update
    void Start()
    {
        startingTime = 60f;
        GameObject timeTextGO = GameObject.Find("TimeLeftText");
        timeText = timeTextGO.GetComponent<Text>();
        
        GameObject goImage = GameObject.Find("GoImage");
        GameObject arrowImage = GameObject.Find("ArrowImage");
        Destroy(goImage, 4);
        Destroy(arrowImage, 4);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
