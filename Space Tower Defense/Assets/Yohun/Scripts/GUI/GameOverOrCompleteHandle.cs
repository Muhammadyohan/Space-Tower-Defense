using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameOverOrCompleteHandle : MonoBehaviour
{
    public UnityEvent GameOverEvent;
    public UnityEvent GameCompleteEvent;

    public void GameOver()
    {
        GameOverEvent.Invoke();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void GameComplete()
    {
        GameCompleteEvent.Invoke();
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
