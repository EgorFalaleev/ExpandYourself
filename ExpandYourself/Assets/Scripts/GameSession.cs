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
    [SerializeField] Text bonusSizeIncreasedText;
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

    // cached references
    RectTransform multiplierTextTransform;

    private void Awake()
    {
        int numberOfGameSessions = FindObjectsOfType<GameSession>().Length;

        // singleton pattern - only one game session can be in the game
        if (numberOfGameSessions > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);

        // disable bonus size text on the start
        bonusSizeText.enabled = false;
        bonusSizeIncreasedText.enabled = false;
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

            // move multipier text to the center of the screen and back
            StartCoroutine(MoveAndScaleMultiplierText(multiplierTextTransform, new Vector2(1280f, 800f), new Vector2(1.75f, 1.75f)));
            pickupsCollectedWithoutMissing = 0;

            if (multiplier <= 7) IncreaseDifficulty();
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
        bonusSizeIncreasedText.enabled = true;

        yield return new WaitForSeconds(3f);

        bonusSizeText.enabled = false;
        bonusSizeIncreasedText.enabled = false;
    }

    private IEnumerator MoveAndScaleMultiplierText(RectTransform textTransform, Vector2 targetPosition, Vector2 targetScale)
    {
        // move text down and left
        textTransform.position = Vector2.Lerp(textTransform.position, targetPosition, 1f);
        textTransform.localScale = targetScale;
        yield return new WaitForSeconds(0.5f);

        multiplierText.text = "x" + multiplier.ToString();

        yield return new WaitForSeconds(0.5f);

        // move text to the previous position
        textTransform.position = Vector2.Lerp(targetPosition, multiplierTextStartingPosition, 1f);
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
