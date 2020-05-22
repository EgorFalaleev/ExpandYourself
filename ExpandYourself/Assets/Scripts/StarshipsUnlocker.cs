using UnityEngine;
using UnityEngine.UI;

public class StarshipsUnlocker : MonoBehaviour
{
    // configuration parameters
    [SerializeField] Text totalPickupsCollectedText;

    // cached references
    GameObject currentStarship;

    // state variables
    private int totalPickupsCollected;
    private int chosenStarship;

    enum Starships
    {
        Starting,
        OneHundred,
        ThreeHundred,
        FiveHundred,
        OneThousand,
        ThreeThousand,
        FiveThousand,
        TenThousand
    }

    void Start()
    {
        // get the total pickups
        totalPickupsCollected = PlayerPrefs.GetInt("TotalPickups");

        totalPickupsCollectedText.text = "You collected " + totalPickupsCollected + " gears!";

        // turn on unlocked starships and turn off blocked starships
        ShowUnlockedStarships(totalPickupsCollected);

        // find the current selected starship
        chosenStarship = PlayerPrefs.GetInt("ChosenStarshipNumber", 0);
        currentStarship = GameObject.Find(GetStarshipName((Starships)chosenStarship));

        // color selected starship to orange and prevent it to being clicked
        currentStarship.GetComponent<Image>().color = new Color(0.91f, 0.7f, 0.435f, 0.537f);
        currentStarship.GetComponent<Button>().interactable = false;
    }

    private string GetStarshipName(Starships starship)
    {
        switch(starship)
        {
            case Starships.OneHundred: return "OneHundred";
            case Starships.ThreeHundred: return "ThreeHundred";
            case Starships.FiveHundred: return "FiveHundred";
            case Starships.OneThousand: return "OneThousand";
            case Starships.ThreeThousand: return "ThreeThousand";
            case Starships.FiveThousand: return "FiveThousand";
            case Starships.TenThousand: return "TenThousand";
            default: return "Starting";
        }
    }

    private void ShowUnlockedStarships(int totalPickups)
    {
        GameObject starshipToCheck;
        GameObject starshipImage;

        // for each starship, check if it is unlocked and color it and turn on/off
        for (int i = 0; i < 8; i++)
        {
            starshipToCheck = GameObject.Find(GetStarshipName((Starships)i));
            starshipImage = GetSharshipImage(i);

            if (totalPickups >= GetStarshipCost(i))
            {
                starshipImage.GetComponent<Image>().color = Color.white;
            }
            else
            {
                starshipImage.GetComponent<Image>().color = Color.black;
                starshipToCheck.GetComponent<Button>().interactable = false;
            }
        }
    }

    private int GetStarshipCost(int starshipNumber)
    {
        switch(starshipNumber)
        {
            case 1: return 100;
            case 2: return 300;
            case 3: return 500;
            case 4: return 1000;
            case 5: return 3000;
            case 6: return 5000;
            case 7: return 10000;
            default: return 0;
        }
    }

    private GameObject GetSharshipImage(int starshipNumber)
    {
        switch (starshipNumber)
        {
            case 1: return GameObject.Find("100 Starship Image");
            case 2: return GameObject.Find("300 Starship Image");
            case 3: return GameObject.Find("500 Starship Image");
            case 4: return GameObject.Find("1000 Starship Image");
            case 5: return GameObject.Find("3000 Starship Image");
            case 6: return GameObject.Find("5000 Starship Image");
            case 7: return GameObject.Find("10000 Starship Image"); 
            default: return GameObject.Find("Starting Starship Image");
        }
    }

    public void ChangeStarship(int starshipNumber)
    {
        // make the button interactable and remove its color
        currentStarship.GetComponent<Button>().interactable = true;
        currentStarship.GetComponent<Image>().color = new Color(1, 1, 1, 0.537f);

        currentStarship = GameObject.Find(GetStarshipName((Starships)starshipNumber));

        // color selected starship to orange and prevent it to being clicked
        currentStarship.GetComponent<Image>().color = new Color(0.91f, 0.7f, 0.435f, 0.537f);
        currentStarship.GetComponent<Button>().interactable = false;

        PlayerPrefs.SetInt("ChosenStarshipNumber", starshipNumber);
    }
}
