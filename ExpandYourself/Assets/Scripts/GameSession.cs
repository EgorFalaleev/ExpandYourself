using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    // configuration parameters
    [SerializeField] Text scoreText;
    [SerializeField] int score = 0;

    private void Awake()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;

        // singleton pattern - only one game session can be in the game
        if (numberOfGameSessions > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        scoreText.text = score.ToString();
    }

    public void AddToScore(int amount)
    {
        // update score information
        score += amount;
        scoreText.text = score.ToString();
    }

    public void ResetGameSession()
    {
        Destroy(gameObject);
    }
}
