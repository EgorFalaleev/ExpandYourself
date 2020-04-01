using System.Collections;
using UnityEngine;

public class SpeedPickup : Pickup
{
    // configuration parameters
    [SerializeField] float speedInfluenceValue = -1f;
    [SerializeField] float pickupActiveTime = 3f;

    // state variables
    bool isCollected;

    private void Awake()
    {
        // set speed influence
        if (tag == "Speed Pickup") speedInfluenceValue = Mathf.Abs(speedInfluenceValue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // change the object state to collected, process speed decreasing
        isCollected = true;
        StartCoroutine(SlowPlayerForTime());
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private IEnumerator SlowPlayerForTime()
    {
        player.HandleSpeedPickup(true, speedInfluenceValue);

        yield return new WaitForSeconds(pickupActiveTime);

        player.HandleSpeedPickup(false, speedInfluenceValue);

        Destroy(gameObject);
    }

    protected override void Shrink()
    {
        if (!isCollected)
        {
            transform.localScale = new Vector2(transform.localScale.x - scalePerFrameDifferenceFactor * Time.deltaTime,
                                       transform.localScale.y - scalePerFrameDifferenceFactor * Time.deltaTime);
            circleScale = transform.localScale;

            // destroy pickup if it becomes too small
            if (transform.localScale.x < scaleToDestroy || transform.localScale.y < scaleToDestroy)
            {
                Destroy(gameObject);
            }
        }
    }
}
