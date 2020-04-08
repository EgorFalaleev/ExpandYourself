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
        // turn off sounds
        if (isSoundsTurnedOn)
        {
            // change text and color of the button
            GameObject.Find("Sounds On/Off Button").GetComponentInChildren<Text>().text = "Off";
            GameObject.Find("Sounds On/Off Button").GetComponent<Image>().color = Color.gray;

            // turn off sounds
            PlayerPrefs.SetFloat("VolumeOnOff", 0f);
        }

        // turn on sounds
        else
        {
            // change text and color of the button
            GameObject.Find("Sounds On/Off Button").GetComponentInChildren<Text>().text = "On";
            GameObject.Find("Sounds On/Off Button").GetComponent<Image>().color = Color.white;

            PlayerPrefs.SetFloat("VolumeOnOff", 0.5f);
        }

        isSoundsTurnedOn = !isSoundsTurnedOn;
    }
}
