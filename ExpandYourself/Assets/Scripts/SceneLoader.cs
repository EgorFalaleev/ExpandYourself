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
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void LoadStarshipsUnlocks()
    {
        SceneManager.LoadScene("UnlocksStarships");
    }

    public void LoadBackgroundsUnlocks()
    {
        SceneManager.LoadScene("UnlocksBackgrounds");
    }
}
