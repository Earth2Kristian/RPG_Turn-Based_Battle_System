using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreenManager : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1.0f;
    }
    public void LoadBattleScene()
    {
        SceneManager.LoadScene(1); 
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadBackToMenu()
    {
        SceneManager.LoadScene(0);
    }

}
