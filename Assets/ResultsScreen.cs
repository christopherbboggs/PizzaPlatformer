using UnityEngine;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
    public Text finalScoreText;
    public Text finalTimeText;

    // Start is called before the first frame update
    void Start()
    {
        // Get the final score and time from the TimeController
       // int finalScore = TimeController.GetFinalScore();
       // int finalTimeLeft = TimeController.GetFinalTimeLeft();

        // Display the score and time on the results screen
       // finalScoreText.text = "Final Score: " + finalScore.ToString();
        //finalTimeText.text = "Time Left: " + finalTimeLeft.ToString() + "s";
    }
}
