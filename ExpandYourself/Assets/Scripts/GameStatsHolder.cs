using UnityEngine;

public class GameStatsHolder : MonoBehaviour
{
    // cached references
    public static GameStatsHolder Instance;

    // state variables
    private int score;

    private void Awake()
    {
        // singleton
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this) Destroy(gameObject);
    }

    public void ResetStatsHolder()
    {
        Destroy(gameObject);
    }

    public void SetScore(int score)
    {
        this.score = score;

        // set high score 
        if (PlayerPrefs.GetInt("HighScore") < score)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    public int GetScore()
    {
        return score;
    }
}
