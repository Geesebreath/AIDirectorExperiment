using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public static PauseMenu SharedInstance;
    [SerializeField]
    GameObject pausePanel, gameOverPanel;
    bool paused, dead;

    private void Awake()
    {
        SharedInstance = this;
    }
    private void Start()
    {
        paused = false;
        dead = false;
    }
    private void Update()
    {
        if(!dead)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                PauseGame();
                
            }
            if (paused)
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else
        {
            Time.timeScale = 0;
            pausePanel.SetActive(false);
            gameOverPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    public void PauseGame()
    {
        paused = !paused;
    }
    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        dead = true;
    }
}
