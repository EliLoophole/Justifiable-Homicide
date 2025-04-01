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

    public void StartGame()
    {
        SceneManager.LoadScene("TestScene");
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
}
