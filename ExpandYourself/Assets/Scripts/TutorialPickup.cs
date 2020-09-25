﻿using UnityEngine;

public class TutorialPickup : MonoBehaviour
{
    // configuration parameters
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

        if (tag == "Speed Pickup" || tag == "Slow Pickup") ChangePlayerSpeed();
        else if (isShrinkPickup) player.DecreaseSize(0.5f);
        else player.IncreaseSize(0.5f);

        Destroy(gameObject);
    }

    private void ChangePlayerSpeed()
    {
        // change speed depending on the type of the pickup collected
        if (tag == "Speed Pickup")
        {
            player.HandleSpeedPickup(true, 15);
        }
        else if (tag == "Slow Pickup")
        {
            player.HandleSpeedPickup(true, 1);
        }

        Invoke("ReturnToNormalSpeed", 1.5f);
    }

    private void ReturnToNormalSpeed()
    {
        player.HandleSpeedPickup(false, 15);
    }
}