using System.Collections;
using UnityEngine;

public class SpeedPickup : Pickup
{
    // configuration parameters
    [SerializeField] float slowSpeed = 1f;
    [SerializeField] float increasedSpeed = 7f;
    [SerializeField] float pickupActiveTime = 3f;

    // state variables
    bool isCollected;

    private void Awake()
    {
        // set speed influence
        if (tag == "Speed Pickup") slowSpeed = Mathf.Abs(slowSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // change the object state to collected, process speed decreasing
        isCollected = true;
        StartCoroutine(ChangePlayerSpeed());
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private IEnumerator ChangePlayerSpeed()
    {
        // change speed depending on the type of the pickup collected
        if (tag == "Speed Pickup") player.HandleSpeedPickup(true, increasedSpeed);
        else player.HandleSpeedPickup(true, slowSpeed);

        yield return new WaitForSeconds(pickupActiveTime);

        if (tag == "Speed Pickup") player.HandleSpeedPickup(false, increasedSpeed);
        else player.HandleSpeedPickup(false, slowSpeed);

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
