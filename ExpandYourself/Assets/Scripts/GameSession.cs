using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    // configuration parameters
    [SerializeField] Text scoreText;
    [SerializeField] Text multiplierText;
    [SerializeField] int pointsToIncreaseMultiplier = 10;

    // state variables
    private int multiplier = 1;
    private int score = 0;
    private float difficulty = 0;
    public int pickupsCollectedWithoutMissing = 0;

    private void Awake()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;

        // singleton pattern - only one game session can be in the game
        if (numberOfGameSessions > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // set texts
        scoreText.text = score.ToString();
        multiplierText.text = "x" + multiplier.ToString();
    }

    private void Update()
    {
        difficulty = Time.time;
        Debug.Log(difficulty);
    }

    public void AddToScore(int amount)
    {
        pickupsCollectedWithoutMissing++;
        UpdateMultiplier();

        // update score information
        score += amount * multiplier;
        scoreText.text = score.ToString();
    }

    public void ResetGameSession()
    {
        Destroy(gameObject);
    }

    private void UpdateMultiplier()
    {
        // increment multiplier and reset pickup counter
        if (pickupsCollectedWithoutMissing == pointsToIncreaseMultiplier)
        {
            multiplier++;
            multiplierText.text = "x" + multiplier.ToString();
            pickupsCollectedWithoutMissing = 0;
        }
    }
}
