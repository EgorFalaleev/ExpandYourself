using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    // state variables
    bool isSoundsTurnedOn;
    string soundsButtonText;
    int soundsButtonColorCode;

    // cached references
    GameSession gameSession;
    GameObject soundsButton;

    private void Start()
    {
        // get object
        gameSession = FindObjectOfType<GameSession>();
        soundsButton = GameObject.Find("Sounds On/Off Button");

        // load button text and color code
        soundsButton.GetComponentInChildren<Text>().text = PlayerPrefs.GetString("SoundsButtonText", "On");
        soundsButtonColorCode = PlayerPrefs.GetInt("SoundsButtonColorCode", 1);

        // color the button depending on the color code
        if (soundsButtonColorCode == 0)
        {
            soundsButton.GetComponent<Image>().color = Color.gray;
            isSoundsTurnedOn = false;
        }
        else
        {
            soundsButton.GetComponent<Image>().color = Color.white;
            isSoundsTurnedOn = true;
        }
    }

    public void TurnOnOffSounds()
    {
        // turn off sounds
        if (isSoundsTurnedOn)
        {
            // change text and color of the button
            soundsButtonText = soundsButton.GetComponentInChildren<Text>().text = "Off";
            soundsButton.GetComponent<Image>().color = Color.gray;

            // used for coloring the button on the start
            soundsButtonColorCode = 0;

            // turn off sounds
            PlayerPrefs.SetFloat("VolumeOnOff", 0f);
        }

        // turn on sounds
        else
        {
            // change text and color of the button
            soundsButtonText = soundsButton.GetComponentInChildren<Text>().text = "On";
            soundsButton.GetComponent<Image>().color = Color.white;

            // used for coloring the button on the start
            soundsButtonColorCode = 1;

            PlayerPrefs.SetFloat("VolumeOnOff", 0.5f);
        }

        SaveOptions();
        isSoundsTurnedOn = !isSoundsTurnedOn;
    }

    private void SaveOptions()
    { 
        // save button text and color code
        PlayerPrefs.SetString("SoundsButtonText", soundsButtonText);
        PlayerPrefs.SetInt("SoundsButtonColorCode", soundsButtonColorCode);
    }
}
