using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void pause() {
        pauseMenu.SetActive(true);
        gameObject.SetActive(false);
        Time.timeScale = 0f; // pauses the game
    }

    public void resume() {
        pauseMenu.SetActive(false);
        gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

    public void quitAndLoadScene(int sceneID) {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }

    public void quitGame() {
        Application.Quit();
    }
}
