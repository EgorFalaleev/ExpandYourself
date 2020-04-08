using UnityEngine;
using UnityEngine.UI;

public class DisplayGameStats : MonoBehaviour
{
    // configuration parameters
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;
    [SerializeField] Text totalPickupsText;

    void Start()
    {
        // display player stats
        scoreText.text = "Your score: " + GameStatsHolder.Instance.GetScore().ToString();
        highScoreText.text = "High score: " + PlayerPrefs.GetInt("HighScore");
        totalPickupsText.text = "Total pickups collected: " + PlayerPrefs.GetInt("TotalPickups");
    }
}
