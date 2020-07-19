using UnityEngine;
using UnityEngine.UI;

public class DisplayGameStats : MonoBehaviour
{
    // configuration parameters
    [SerializeField] Text scoreText;
    [SerializeField] Text totalPickupsText;
    [SerializeField] Text loseText;

    void Start()
    {
        // display player stats
        scoreText.text = "Your score: " + GameStatsHolder.Instance.GetScore().ToString() + " (Best: " + PlayerPrefs.GetInt("HighScore") + ")";
        totalPickupsText.text = "Total gears collected: " + PlayerPrefs.GetInt("TotalPickups");

        // display lose text depending on player score
        if (GameStatsHolder.Instance.GetScore() <= 100) loseText.text = "Better luck next time!";
        else if (GameStatsHolder.Instance.GetScore() <= 350) loseText.text = "Well done!";
        else loseText.text = "You are awesome!";
    }
}
