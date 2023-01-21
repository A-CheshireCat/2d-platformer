using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsGamePaused = false;
    public GameObject pauseMenuUI;
    private OptionsMenu optionsMenu;

    private void Start() {
        optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu").GetComponent<OptionsMenu>();
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (IsGamePaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    protected void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
    }

    public void GoToOptions() {
        if (optionsMenu) {
            optionsMenu.optionsMenuUI.SetActive(true);
            optionsMenu.LoadOptions();
        }
    }

    public void GoToMainMenu() {
        Resume();
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        Debug.Log("PauseMenu - Application.Quit();");
        Application.Quit();
    }
}
