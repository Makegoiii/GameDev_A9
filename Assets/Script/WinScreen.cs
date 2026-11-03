using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
    }

    public void ToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PlayAgain()
    {
    SceneManager.LoadScene(1);
    }
}
