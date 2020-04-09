using UnityEngine;

public class NegativePickup : Pickup
{
    // configuration parameters
    [SerializeField] float decreasingValue = 0.5f;
    [SerializeField] AudioClip negativePickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.DecreaseSize(decreasingValue);
        AudioSource.PlayClipAtPoint(negativePickupSound, Camera.main.transform.position, PlayerPrefs.GetFloat("VolumeOnOff", 0.5f));
        Destroy(gameObject);
    }

    protected override void Shrink()
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
