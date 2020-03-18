using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
