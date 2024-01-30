using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MaineMenu : MonoBehaviour
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
            //UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
    }
}