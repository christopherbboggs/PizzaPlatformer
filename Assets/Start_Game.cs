using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_Game : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnPlayAgainButton()
    {
        SceneManager.LoadScene("Level 1 proto");
    }
}