using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    // configuration parameters
    [SerializeField] Slider slider;
    [SerializeField] bool isTotalPickupsChecked = true;

    private void Start()
    {
        // set the value of progress bar depending on the property it requires
        if (isTotalPickupsChecked) SetScore(PlayerPrefs.GetInt("TotalPickups", 0));
        else SetScore(PlayerPrefs.GetInt("HighScore", 0));
    }

    public void SetScore(int score)
    {
        if (score > slider.maxValue) Destroy(gameObject);

        slider.value = score;        
    }
}
