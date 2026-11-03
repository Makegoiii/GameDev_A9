using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    public void OpenPanel(GameObject panel)
{
    panel.SetActive(true);
}

public void ClosePanel(GameObject panel)
{
    panel.SetActive(false);
}
    public GameObject settingsPanel;
    public AudioSource bgSound;

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel(int index)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(index);
    }

    public void Settings()
    {
        settingsPanel.SetActive(true);
    }

    public void Back()
    {
        settingsPanel.SetActive(false);
    }

    public void SetVolume(float value)
    {
        bgSound.volume = value;
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quitting the game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
