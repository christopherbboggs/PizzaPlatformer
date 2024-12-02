using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour
{
    Text finalScoreText;
    // Start is called before the first frame update
    void Start()
    {
        GameObject finalScoreTextGO = GameObject.Find("FinalScore");
        finalScoreText = finalScoreTextGO.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        finalScoreText.text = string.Format("Final Score: {0:n0}", PizzaManager.currentScore);
    }
}
