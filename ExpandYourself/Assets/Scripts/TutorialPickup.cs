using System.Collections;
using UnityEngine;

public class TutorialPickup : MonoBehaviour
{
    // configuration parameters
    [SerializeField] private bool isSlowPickup;
    [SerializeField] private bool isSpeedPickup;
    [SerializeField] private bool isShrinkPickup;
    [SerializeField] private GameObject collectedVFX;

    // cached references
    private Player player;
    private TutorialManager tutorialManager;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        tutorialManager = FindObjectOfType<TutorialManager>();
        tutorialManager.playerHasCollectedPickup = true;

        // create particles
        GameObject normalPickupParticles = Instantiate(collectedVFX, transform.position, transform.rotation);
        Destroy(normalPickupParticles, 1f);

        if (isSlowPickup || isSpeedPickup) StartCoroutine(ChangePlayerSpeed());
        else if (isShrinkPickup) player.DecreaseSize(0.5f);
        else player.IncreaseSize(0.5f);

        Destroy(gameObject);
    }

    private IEnumerator ChangePlayerSpeed()
    {
        // change speed depending on the type of the pickup collected
        if (isSpeedPickup)
        {
            player.HandleSpeedPickup(true, 15);
        }
        else if (isSlowPickup)
        {
            player.HandleSpeedPickup(true, 1);
        }

        yield return new WaitForSeconds(1.5f);

        if (isSpeedPickup) player.HandleSpeedPickup(false, 15);
        else if (isSlowPickup) player.HandleSpeedPickup(false, 1);
    }
}
