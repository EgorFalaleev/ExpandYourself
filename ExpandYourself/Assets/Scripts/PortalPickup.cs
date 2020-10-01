using UnityEngine;

public class PortalPickup : Pickup
{
    [SerializeField] AudioClip portalPickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.TeleportAfterPortal();
        AudioSource.PlayClipAtPoint(portalPickupSound, Camera.main.transform.position, PlayerPrefs.GetFloat("VolumeOnOff", 0.5f));
        Destroy(gameObject);

        // create particles
        GameObject speedPickupParticles = Instantiate(collectedVFX, transform.position, transform.rotation);
        Destroy(speedPickupParticles, 1f);
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