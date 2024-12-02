using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{

    Text timeText;
    float startingTime;
    int timeLeft;
    Text scoreText;
    Text pointsEarnedText;
    Text trickText;
    Text boostText;


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

        scoreText = GameObject.Find("ActualScoreText").GetComponent<Text>();
        scoreText.text = "0";

        pointsEarnedText = GameObject.Find("PointsEarnedText").GetComponent<Text>();
        pointsEarnedText.text = "";

        trickText = GameObject.Find("TrickTypeText").GetComponent<Text>();
        trickText.text = "";

        boostText = GameObject.Find("BoostText").GetComponent <Text>();
        boostText.text = "";

    }

    // Update is called once per frame
    void Update()
    {

        float timePassed = Time.unscaledTime;
        float temp = startingTime - timePassed;
        timeLeft = (int)temp;

        if (timeLeft >= 0)
        {
            timeText.text = timeLeft.ToString();
        }
    }
}
