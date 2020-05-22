using UnityEngine;
using UnityEngine.UI;

public class BackgroundsUnlocker : MonoBehaviour
{
    // configuration parameters
    [SerializeField] Text highScoreText;

    // cached references
    GameObject currentBackground;

    // state variables
    private int highScore;
    private int chosenBackground;

    enum Backgrounds
    {
        Starting,
        TwoHundred,
        ThreeHundredFifty,
        FiveHundred,
    }

    void Start()
    {
        // get high score
        highScore = PlayerPrefs.GetInt("HighScore");

        highScoreText.text = $"Your high score is {highScore}!";

        // find the current selected background
        chosenBackground = PlayerPrefs.GetInt("ChosenBackgroundNumber", 0);
        currentBackground = GameObject.Find(GetStarshipName((Backgrounds)chosenBackground));

        // color selected background to orange and prevent it to being clicked
        currentBackground.GetComponent<Image>().color = new Color(0.91f, 0.7f, 0.435f, 0.537f);
        currentBackground.GetComponent<Button>().interactable = false;
    }

    private string GetStarshipName(Backgrounds background)
    {
        switch (background)
        {
            case Backgrounds.TwoHundred: return "TwoHundred";
            case Backgrounds.ThreeHundredFifty: return "ThreeHundredFifty";
            case Backgrounds.FiveHundred: return "FiveHundred";
            default: return "Starting";
        }
    }

    public void ChangeBackground(int backgroundNumber)
    {
        // make the button interactable and remove its color
        currentBackground.GetComponent<Button>().interactable = true;
        currentBackground.GetComponent<Image>().color = new Color(1, 1, 1, 0.537f);

        currentBackground = GameObject.Find(GetStarshipName((Backgrounds)backgroundNumber));

        // color selected background to orange and prevent it to being clicked
        currentBackground.GetComponent<Image>().color = new Color(0.91f, 0.7f, 0.435f, 0.537f);
        currentBackground.GetComponent<Button>().interactable = false;

        PlayerPrefs.SetInt("ChosenBackgroundNumber", backgroundNumber);
    }
}
