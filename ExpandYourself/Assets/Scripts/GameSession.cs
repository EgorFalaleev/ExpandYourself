using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    // configuration parameters
    [SerializeField] Text scoreText;
    [SerializeField] Text multiplierText;
    [SerializeField] Text maxSizeText;
    [SerializeField] int pointsToIncreaseMultiplier = 10;
    [SerializeField] int maxSizeBonus = 10;
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

        // disable max size text on the start
        maxSizeText.enabled = false;
    }

    private void Start()
    {
        // set texts
        scoreText.text = score.ToString();
        multiplierText.text = "x" + multiplier.ToString();
        maxSizeText.enabled = false;
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

    public void ProcessMaxSizeReached()
    {
        // add bonus points and show text
        score += maxSizeBonus;
        StartCoroutine(ShowMaxSizeText());
    }

    private IEnumerator ShowMaxSizeText()
    {
        // show text then destroy it
        maxSizeText.text = $"Max size reached! + {maxSizeBonus} points!";
        maxSizeText.enabled = true;
        
        yield return new WaitForSeconds(2f);

        maxSizeText.enabled = false;
    }
}
