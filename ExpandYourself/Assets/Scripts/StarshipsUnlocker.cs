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
