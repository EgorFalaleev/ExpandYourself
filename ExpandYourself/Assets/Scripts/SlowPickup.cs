using System.Collections;
using UnityEngine;

public class SlowPickup : Pickup
{
    // configuration parameters
    [SerializeField] float slowDownValue = -1f;
    [SerializeField] float timeForSlowDown = 3f;

    // state variables
    bool isCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // change the object state to collected, process speed decreasing
        isCollected = true;
        StartCoroutine(SlowPlayerForTime());
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private IEnumerator SlowPlayerForTime()
    {
        player.HandleSpeedPickup(true, slowDownValue);

        yield return new WaitForSeconds(timeForSlowDown);

        player.HandleSpeedPickup(false, slowDownValue);

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
