using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGenerator : MonoBehaviour
{
    // configuration parameters
    [SerializeField] GameObject[] pickupPrefab;
    [SerializeField] float timeBetweenSpawns = 1f;
    [SerializeField] float minPickupScale = 0.75f;
    [SerializeField] float maxPickupScale = 2f;
    [Range (80, 100)]
    [SerializeField] int negativePickupProbability = 100;

    // state variables
    private Vector2 screenBounds;
    private Vector2 pickupScale;
    private int neutralPickupProbability = 100;

    private void Start()
    {
        // get screen bounds
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        StartCoroutine(SpawnCoroutine());
    }

    private void SpawnPickup()
    {
        SetPickupProbabilities();

        // generate probability for each pickup collected
        int pickupIndex = 0;
        int probability = Random.Range(0, 100);

        // select pickup type depending on probability
        if (probability <= neutralPickupProbability) pickupIndex = 0;
        else if (probability > neutralPickupProbability && probability <= negativePickupProbability) pickupIndex = 1;

        // spawn a pickup
        GameObject pickup = Instantiate(pickupPrefab[pickupIndex]) as GameObject;

        float randomScale = Random.Range(minPickupScale, maxPickupScale);

        // place a pickup on a random position within the screen and make its size random
        pickup.transform.position = new Vector2(Random.Range(screenBounds.x - randomScale, -screenBounds.x + randomScale),
                                                Random.Range(screenBounds.y - randomScale, -screenBounds.y + randomScale));
        pickup.transform.localScale = new Vector2(randomScale, randomScale);
        pickupScale = pickup.transform.localScale;
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);
            SpawnPickup();
        }
    }

    public Vector2 GetPickupScale()
    {
        return pickupScale;
    }

    public void DecreaseTimeBetweenSpawns(float value)
    {
        timeBetweenSpawns -= value;
    }

    private void SetPickupProbabilities()
    {
        // set the probabilities for the neutral pickup depending on the multiplier
        switch (FindObjectOfType<GameSession>().GetMultiplier())
        {
            case 1:
                neutralPickupProbability = 100;
                break;
            case 2:
                neutralPickupProbability = 90;
                break;
            case 3:
                neutralPickupProbability = 80;
                break;
            case 4:
                neutralPickupProbability = 70;
                break;
            default:
                neutralPickupProbability = 65;
                break;
        }
    }
}
