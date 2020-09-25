using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    // configuration parameters
    [Header("Text references")]
    [SerializeField] Text scoreText;
    [SerializeField] Text multiplierText;
    [SerializeField] Text bonusSizeText;
    [Header("Pickup settings")]
    [SerializeField] int pointsToIncreaseMultiplier = 10;
    [SerializeField] int bonusSizePoints = 10;
    [SerializeField] float playerDecreasingSizeValue = 0.5f;
    [SerializeField] float pickupDecreasingSizeValue = 0.3f;
    [SerializeField] float decreasingTimeBetweenSpawnsValue = 0.3f;
    [SerializeField] AudioClip[] pickupCollectedSounds;
    [Range(0, 1)]
    [SerializeField] float pickupCollectedVolume = 0.5f;

    // state variables
    private int multiplier = 1;
    private int score = 0;
    private int pickupsCollectedWithoutMissing = 0;
    private int totalPickupsCollected = 0;
    private Vector2 multiplierTextStartingPosition;
    private Vector2 multiplierTextStartingScale;
    private bool isMultiplierIncreased = false;
    private bool isMultiplierOnCenter = false;

    // cached references
    RectTransform multiplierTextTransform;

    private void Awake()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;

        // singleton pattern - only one game session can exist in the game
        if (numberOfGameSessions > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);

        // disable bonus size text on the start
        bonusSizeText.enabled = false;
    }

    private void Start()
    {
        // get reference to the multiplier text transform;
        multiplierTextTransform = GameObject.Find("Multiplier Text").GetComponent<RectTransform>();

        // get starting position and scale of multiplier text
        multiplierTextStartingPosition = multiplierTextTransform.position;
        multiplierTextStartingScale = multiplierTextTransform.localScale;

        // set texts
        scoreText.text = score.ToString();
        multiplierText.text = "x" + multiplier.ToString();

        // get number of total pickups collected
        totalPickupsCollected = PlayerPrefs.GetInt("TotalPickups");

        // get volume settings
        pickupCollectedVolume = PlayerPrefs.GetFloat("VolumeOnOff", 0.5f);
    }

    private void Update()
    {
        // when multiplier increases, move it to center and back
        if (isMultiplierIncreased) MoveMultiplierToCenter(multiplierTextTransform, new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(1.75f, 1.75f));
        else if (isMultiplierOnCenter) MoveMultiplierToStartPosition(multiplierTextTransform);
    }
    
    public void AddToScore(int amount)
    {
        // update stats info
        totalPickupsCollected++;
        pickupsCollectedWithoutMissing++;

        // play pickup collected sound
        AudioSource.PlayClipAtPoint(pickupCollectedSounds[pickupsCollectedWithoutMissing - 1], Camera.main.transform.position, pickupCollectedVolume);

        UpdateMultiplier();

        // update score information
        score += amount * multiplier;
        scoreText.text = score.ToString();
    }

    public void ResetGameSession()
    {
        SaveGameStats();

        Destroy(gameObject);
    }

    public void ResetNumberOfPickupsCollected()
    {
        pickupsCollectedWithoutMissing = 0;
    }

    private void UpdateMultiplier()
    {
        // increment multiplier and reset pickup counter
        if (pickupsCollectedWithoutMissing == pointsToIncreaseMultiplier)
        {
            FindObjectOfType<NormalPickupParticlesHandler>().IncreaseNumberOfParticles();
            multiplier++;

            isMultiplierIncreased = true;

            pickupsCollectedWithoutMissing = 0;

            if (multiplier <= 7) IncreaseDifficulty();
            if (multiplier == 10) IncreaseDifficulty();
        }
    }

    private void IncreaseDifficulty()
    {
         // increase difficulty by speeding up the player's and pickup's shrinking speed
         FindObjectOfType<Player>().AcceleratePlayerShrinking(playerDecreasingSizeValue);
         FindObjectOfType<Pickup>().AcceleratePickupShrinking(pickupDecreasingSizeValue);
         
         // decrease time between pickups spawns
         FindObjectOfType<PickupGenerator>().DecreaseTimeBetweenSpawns(decreasingTimeBetweenSpawnsValue);
    }

    public void ProcessBonusSizeReached()
    {
        // add bonus points and show text
        score += bonusSizePoints;
        scoreText.text = score.ToString();
        StartCoroutine(ShowBonusSizeText());
        bonusSizePoints += 2;
    }

    private IEnumerator ShowBonusSizeText()
    {
        // show text then destroy it
        bonusSizeText.text = $"Bonus size reached! + {bonusSizePoints} points!";
        bonusSizeText.enabled = true;

        yield return new WaitForSeconds(3.5f);

        bonusSizeText.enabled = false;
    }

    private void MoveMultiplierToCenter(RectTransform textTransform, Vector2 targetPosition, Vector2 targetScale)
    {
        // move text down and left
        textTransform.position = Vector2.Lerp(textTransform.position, targetPosition, 0.2f);
        textTransform.localScale = targetScale;
        multiplierText.text = "x" + multiplier.ToString();

        Invoke("ChangeMultiplierMoveBehavior", 1f);
    }

    private void ChangeMultiplierMoveBehavior()
    {
        isMultiplierIncreased = false;
        isMultiplierOnCenter = true;
    }

    private void MoveMultiplierToStartPosition(RectTransform textTransform)
    {
        // move text to starting position
        textTransform.position = Vector2.Lerp(textTransform.position, multiplierTextStartingPosition, 0.2f);
        textTransform.localScale = multiplierTextStartingScale;
    }

    public int GetMultiplier()
    {
        return multiplier;
    }

    public void SaveGameStats()
    {
        // save score and number of pickups
        GameStatsHolder.Instance.SetScore(score);
        GameStatsHolder.Instance.SetTotalPickups(totalPickupsCollected);
    }
}
