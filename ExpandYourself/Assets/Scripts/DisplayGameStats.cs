using UnityEngine;
using UnityEngine.UI;

public class DisplayGameStats : MonoBehaviour
{
    // configuration parameters
    [SerializeField] Text scoreText;

    void Start()
    {
        scoreText.text = "Your score: " + GameStatsHolder.Instance.GetScore().ToString();
    }
}
