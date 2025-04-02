using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject loseUI;

    [SerializeField]
    private GameObject winUI;

    [SerializeField]
    private GameObject pauseUI;


    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && pauseUI.activeSelf == false)
        {
            Pause();
        } else if (Input.GetKeyUp(KeyCode.Escape) && pauseUI.activeSelf == true)
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void Lose()
    {
        Time.timeScale = 0.03f;
        loseUI.SetActive(true);
    }

    public void Win()
    {
        Time.timeScale = 0.03f;
        winUI.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextStage()
    {
        Time.timeScale = 1.0f;
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }

    public void ExitGame()
    {
        print("exit");
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseUI.SetActive(true);
    }
}
