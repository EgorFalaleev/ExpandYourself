using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popups;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        player.tutorialMode = true;
    }
}
