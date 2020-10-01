using System.Collections;
using UnityEngine;

public class TutorialPickup : MonoBehaviour
{
    // configuration parameters
    [SerializeField] private bool isShrinkPickup;
    [SerializeField] private bool isPortalPickup;
    [SerializeField] private GameObject collectedVFX;

    // cached references
    private Player player;
    private TutorialManager tutorialManager;

    // state variables
    private int speedInfluence = 0;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
  //      tutorialManager.playerHasCollectedPickup = true;

        // create particles
        GameObject normalPickupParticles = Instantiate(collectedVFX, transform.position, transform.rotation);
        Destroy(normalPickupParticles, 1f);

        if (tag == "Speed Pickup" || tag == "Slow Pickup") StartCoroutine(ChangePlayerSpeed());
        else if (isShrinkPickup)
        {
            player.DecreaseSize(0.5f);
            tutorialManager.playerHasCollectedPickup = true;
            Destroy(gameObject);
        }
        else if (isPortalPickup)
        {
            player.TeleportAfterPortal();
            tutorialManager.playerHasCollectedPickup = true;
            Destroy(gameObject);
        }
        else
        {
            player.IncreaseSize(0.5f);
            tutorialManager.playerHasCollectedPickup = true;
            Destroy(gameObject);
        }
    }

    private IEnumerator ChangePlayerSpeed()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        // change speed depending on the type of the pickup collected
        if (tag == "Speed Pickup")
        {
            speedInfluence = 15;
            player.HandleSpeedPickup(true, speedInfluence);
        }
        else if (tag == "Slow Pickup")
        {
            speedInfluence = 1;
            player.HandleSpeedPickup(true, speedInfluence);
        }

        yield return new WaitForSeconds(1.5f);

        player.HandleSpeedPickup(false, speedInfluence);
        tutorialManager.playerHasCollectedPickup = true;
        Destroy(gameObject);
    }
}