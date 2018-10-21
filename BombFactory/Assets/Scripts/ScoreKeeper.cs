using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreKeeper : MonoBehaviour {

    public static int score = 0;
    public static bool gameOver = false;

    static UnityEngine.UI.Text scoreText = null;
    static UnityEngine.UI.Button resetButton = null;
    static UnityEngine.UI.Button quitButton = null;

    private void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<UnityEngine.UI.Text>();
        resetButton = GameObject.Find("ResetButton").GetComponent<UnityEngine.UI.Button>();
        resetButton.gameObject.SetActive(false);
        quitButton = GameObject.Find("QuitButton").GetComponent<UnityEngine.UI.Button>();
        quitButton.gameObject.SetActive(false);
    }

    public static void AddToScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }

    public static void EndGame()
    {
        gameOver = true;
        resetButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    public static void ResetGame()
    {
        score = 0;
        gameOver = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetGameHandler()
    {
        ResetGame();
        SceneManager.LoadScene(0);
    }
}
