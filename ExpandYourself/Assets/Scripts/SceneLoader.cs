using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void LoadGameScene()
    {
        if (PlayerPrefs.GetInt("TotalPickups", 0) == 0) SceneManager.LoadScene("Tutorial");
        else SceneManager.LoadScene("GameScene");
    }

    public void LoadGameAfterTutorial()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadStarshipsUnlocks()
    {
        SceneManager.LoadScene("UnlocksStarships");
    }

    public void LoadBackgroundsUnlocks()
    {
        SceneManager.LoadScene("UnlocksBackgrounds");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
