using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // configuration parameters
    public GameObject[] popups;
    public GameObject[] pickups;
    public GameObject playButton;

    // cached references
    private Player player;
    private Pickup pickup;

    // state variables
    private int currentPopupIndex = 0;
    public bool playerHasMoved = false;
    public bool playerHasCollectedPickup = false;

    private void Start()
    {
        // turn on player's tutorial mode to prevent him from shrinking
        player = FindObjectOfType<Player>();
        player.tutorialMode = true;
    }

    private void Update()
    {
        for (int i = 0; i < popups.Length; i++)
        {
            if (i == currentPopupIndex) popups[i].SetActive(true);
            else popups[i].SetActive(false);
        }

        switch (currentPopupIndex)
        {
            // learning how to move, turning on normal pickup
            case 0:
                if (playerHasMoved)
                {
                    currentPopupIndex++;
                    pickups[0].SetActive(true);
                }
                break;
            // learning normal pickup collection, turning on speed pickup
            case 1:
                if (playerHasCollectedPickup)
                {
                    currentPopupIndex++;
                    pickups[1].SetActive(true);
                    playerHasCollectedPickup = false;
                }
                break;
            // learning speed pickup collection, turning on slow pickup
            case 2:
                if (playerHasCollectedPickup)
                {
                    currentPopupIndex++;
                    pickups[2].SetActive(true);
                    playerHasCollectedPickup = false;
                }
                break;
            // learning slow pickup collection, turning on shrink pickup
            case 3:
                if (playerHasCollectedPickup)
                {
                    currentPopupIndex++;
                    pickups[3].SetActive(true);
                    playerHasCollectedPickup = false;
                }
                break;
            // learning shrink pickup collection, turning on portal pickup
            case 4:
                if (playerHasCollectedPickup)
                {
                    currentPopupIndex++;
                    pickups[4].SetActive(true);
                    playerHasCollectedPickup = false;
                }
                break;
            // learning portal pickup
            case 5:
                if (playerHasCollectedPickup)
                {
                    currentPopupIndex++;
                }
                break;
            case 6:
                playButton.SetActive(true);
                break;
        }
    }
}