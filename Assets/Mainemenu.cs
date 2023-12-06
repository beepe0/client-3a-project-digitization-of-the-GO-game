using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainemenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("test_scene");
    }
    public void SettingsScene()
    {
        SceneManager.LoadScene("SettingsScene");
    }
    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
