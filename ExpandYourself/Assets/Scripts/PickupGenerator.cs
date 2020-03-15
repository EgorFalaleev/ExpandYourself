using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGenerator : MonoBehaviour
{
    // configuration parameters
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] float timeBetweenSpawns = 1f;
    [SerializeField] float minPickupScale = 0.75f;
    [SerializeField] float maxPickupScale = 2f;

    // state variables
    private Vector2 screenBounds;
    private Vector2 pickupScale;

    void Start()
    {
        // get screen bounds
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        StartCoroutine(SpawnCoroutine());
    }

    private void SpawnPickup()
    {
        // spawn a pickup
        GameObject pickup = Instantiate(pickupPrefab) as GameObject;

        float randomScale = Random.Range(minPickupScale, maxPickupScale);

        // place a pickup on a random position within the screen and make its size random
        pickup.transform.position = new Vector2(Random.Range(screenBounds.x, -screenBounds.x), Random.Range(screenBounds.y, -screenBounds.y));
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
}
