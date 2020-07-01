using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    private void Start()
    {
        // load currently selected background
        GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{PlayerPrefs.GetString("BackgroundSprite", "Starting Background")}");
    }

    public void ChangeBackgroundImage()
    {
        // load currently selected background
        GetComponent<Image>().sprite = Resources.Load<Sprite>($"Sprites/{PlayerPrefs.GetString("BackgroundSprite", "Starting Background")}");
    }
}
