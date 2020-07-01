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

        // turn on unlocked starships and turn off blocked starships
        ShowUnlockedBackgrounds(highScore);

        // find the current selected background
        chosenBackground = PlayerPrefs.GetInt("ChosenBackgroundNumber", 0);
        currentBackground = GameObject.Find(GetBackgroundName((Backgrounds)chosenBackground));

        // color selected background to orange and prevent it to being clicked
        currentBackground.GetComponent<Image>().color = new Color(0.91f, 0.7f, 0.435f, 0.537f);
        currentBackground.GetComponent<Button>().interactable = false;
    }

    private void ShowUnlockedBackgrounds(int highScore)
    {
        GameObject backgroundToCheck;
        GameObject backgroundImage;

        // for each starship, check if it is unlocked and color it and turn on/off
        for (int i = 0; i < 4; i++)
        {
            backgroundToCheck = GameObject.Find(GetBackgroundName((Backgrounds)i));
            backgroundImage = GetBackgroundImage(i);

            if (highScore >= GetBackgroundCost(i))
            {
                backgroundImage.GetComponent<Image>().color = Color.white;
            }
            else
            {
                backgroundImage.GetComponent<Image>().color = Color.black;
                backgroundToCheck.GetComponent<Button>().interactable = false;
            }
        }
    }

    private int GetBackgroundCost(int starshipNumber)
    {
        switch (starshipNumber)
        {
            case 1: return 200;
            case 2: return 350;
            case 3: return 500;
            default: return 0;
        }
    }

    private GameObject GetBackgroundImage(int starshipNumber)
    {
        switch (starshipNumber)
        {
            case 1: return GameObject.Find("200 background Image");
            case 2: return GameObject.Find("300 background Image");
            case 3: return GameObject.Find("500 background Image");
            default: return GameObject.Find("Starting Background Image");
        }
    }

    private string GetBackgroundName(Backgrounds background)
    {
        switch (background)
        {
            case Backgrounds.TwoHundred: return "TwoHundred";
            case Backgrounds.ThreeHundredFifty: return "ThreeHundredFifty";
            case Backgrounds.FiveHundred: return "FiveHundred";
            default: return "Starting";
        }
    }

    private string GetBackgroundName(int backgroundNumber)
    {
        switch (backgroundNumber)
        {
            case 1: return "200 background";
            case 2: return "350 background";
            case 3: return "500 background";
            default: return "Starting Background";
        }
    }

    public void ChangeBackground(int backgroundNumber)
    {
        // make the button interactable and remove its color
        currentBackground.GetComponent<Button>().interactable = true;
        currentBackground.GetComponent<Image>().color = new Color(1, 1, 1, 0.537f);

        currentBackground = GameObject.Find(GetBackgroundName((Backgrounds)backgroundNumber));

        // color selected background to orange and prevent it to being clicked
        currentBackground.GetComponent<Image>().color = new Color(0.91f, 0.7f, 0.435f, 0.537f);
        currentBackground.GetComponent<Button>().interactable = false;

        PlayerPrefs.SetInt("ChosenBackgroundNumber", backgroundNumber);

        UpdateBackground(backgroundNumber);

        FindObjectOfType<BackgroundChanger>().ChangeBackgroundImage();
    }

    private void UpdateBackground(int backgroundNumber)
    {
        // set new background
        PlayerPrefs.SetString("BackgroundSprite", GetBackgroundName(backgroundNumber));
    }
}
