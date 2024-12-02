using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{

    Text timeText;
    public float startingTime = 60f;
    public static float timeLeft;
    Text scoreText;
    Text pointsEarnedText;
    Text trickText;
    Text boostText;

    public int secondsPenalized; // positive value means lost time



    // Start is called before the first frame update
    void Start()
    {
        timeLeft = startingTime;
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

        boostText = GameObject.Find("BoostText").GetComponent<Text>();
        boostText.text = "";

        GameObject playerGO = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        float actualTime = timeLeft - secondsPenalized;

        if (actualTime >= 0)
        {
            timeText.text = String.Format("{0:0}", actualTime);

            UpdatePizzaHealth(actualTime);
        }
        else
        {
            timeText.text = "0";
            UpdatePizzaHealth(actualTime);
            SceneManager.LoadScene("Results_Screen");
        }
    }

    int GetCurrentScore()
    {
        int temp1 = PizzaManager.currentScore;
        return temp1;
    }

    public void UpdateScoreUI(int tricksJustLanded)
    {
        if (tricksJustLanded <= 0)
        {
            pointsEarnedText.text = "";
        }
        else
        {
            int pointsGained = tricksJustLanded * 100;
            pointsEarnedText.text = "+" + pointsGained.ToString();

            int theCurrentScore = GetCurrentScore();
            PizzaManager.currentScore = theCurrentScore + pointsGained;
            scoreText.text = (theCurrentScore + pointsGained).ToString();
        }

    }

    string TrickToString(TrickController.TrickState givenTrick)
    {
        string tempString;

        if (givenTrick == TrickController.TrickState.BACKFLIP)
        {
            tempString = "Backflip";
        }
        else if (givenTrick == TrickController.TrickState.FRONTFLIP)
        {
            tempString = "Frontflip";
        }
        else if (givenTrick == TrickController.TrickState.SPIN_LEFT)
        {
            tempString = "Left Spin";
        }
        else if (givenTrick == TrickController.TrickState.SPIN_RIGHT)
        {
            tempString = "Right Spin";
        }
        else
        {
            tempString = "Idle";
        }

        return tempString;
    }

    public void UpdateTrickUI(List<TrickController.TrickState> tricksDone, bool trickFailed)
    {
        if (trickFailed)
        {
            trickText.text = "";
        }
        else
        {
            string trickString;
            int frequency = 0;
            TrickController.TrickState trickKey = tricksDone[tricksDone.Count - 1];

            foreach (TrickController.TrickState trick in tricksDone)
            {
                if (trick == trickKey)
                {
                    frequency++;
                }
            }

            trickString = TrickToString(trickKey);
            trickText.text = trickString + " x " + frequency.ToString();
        }

        return;
    }

    public void UpdateBoostUI(int tricksCompleted)
    {
        if (tricksCompleted == 0)
        {
            boostText.text = "";
        }
        else
        {
            boostText.text = "Boost x " + tricksCompleted.ToString();
        }

        return;
    }

    public void RemoveTime(int seconds)
    {
        secondsPenalized += seconds;
        return;
    }

    public void UpdatePizzaHealth(float timeRemaining)
    {
        GameObject meterholderGO = GameObject.Find("MeterHolderObject");
        RectMask2D mask = meterholderGO.GetComponent<RectMask2D>();
        Vector4 vectourFour = mask.padding;
        vectourFour.z = 135 + (11 * (startingTime - timeRemaining)); // z field is the right side padding of mask
        mask.padding = vectourFour;
        return;

    }
}