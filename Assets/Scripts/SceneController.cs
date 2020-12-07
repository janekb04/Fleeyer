using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    static string previousSceneName = string.Empty;

    public void MainMenu()
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("MainMenu");
    }

    public void MainLevel()
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("MainLevel");
    }

    public void Shop()
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Shop", LoadSceneMode.Additive);
    }

    public void Leaderboard()
    {
        previousSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Leaderboard", LoadSceneMode.Additive);
    }

    public void PreviousScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(previousSceneName);
        previousSceneName = currentSceneName;
    }

    public void UnloadScene(GameObject sender)
    {
        SceneManager.UnloadSceneAsync(sender.scene);
    }

    public void Exit()
    {
        Debug.Log("Application Quit");
        Application.Quit();
    }
}
