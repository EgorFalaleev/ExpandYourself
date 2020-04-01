using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    // configuration parameters
    [SerializeField] Text scoreText;
    [SerializeField] Text multiplierText;
    [SerializeField] Text bonusSizeText;
    [SerializeField] Text bonusSizeIncreasedText;
    [SerializeField] int pointsToIncreaseMultiplier = 10;
    [SerializeField] int bonusSizePoints = 10;
    [SerializeField] float playerDecreasingSizeValue = 0.5f;
    [SerializeField] float pickupDecreasingSizeValue = 0.3f;
    [SerializeField] float decreasingTimeBetweenSpawnsValue = 0.3f;

    // state variables
    private int multiplier = 1;
    private int score = 0;
    private int pickupsCollectedWithoutMissing = 0;

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
        // set texts
        scoreText.text = score.ToString();
        multiplierText.text = "x" + multiplier.ToString();
    }

    public void AddToScore(int amount)
    {
        pickupsCollectedWithoutMissing++;
        UpdateMultiplier();

        // update score information
        score += amount * multiplier;
        scoreText.text = score.ToString();
    }

    public void ResetGameSession()
    {
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
            multiplier++;
            multiplierText.text = "x" + multiplier.ToString();
            IncreaseDifficulty();
            pickupsCollectedWithoutMissing = 0;
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

        yield return new WaitForSeconds(2f);

        bonusSizeText.enabled = false;
        bonusSizeIncreasedText.enabled = false;
    }

    public int GetMultiplier()
    {
        return multiplier;
    }
}
