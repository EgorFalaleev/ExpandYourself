using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    // state variables
    bool isSoundsTurnedOn = true;

    // cached references
    GameSession gameSession;

    private void Start()
    {
        // get object
        gameSession = FindObjectOfType<GameSession>();
    }

    public void TurnOnOffSounds()
    {
        if (isSoundsTurnedOn)
        {
            GameObject.Find("Sounds On/Off Button").GetComponentInChildren<Text>().text = "Off";
            GameObject.Find("Sounds On/Off Button").GetComponent<Image>().color = Color.gray;
        }
        else
        {
            GameObject.Find("Sounds On/Off Button").GetComponentInChildren<Text>().text = "On";
            GameObject.Find("Sounds On/Off Button").GetComponent<Image>().color = Color.white;
        }

        isSoundsTurnedOn = !isSoundsTurnedOn;
    }
}
