using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button continueButton;

    private OptionsMenu optionsMenu;
    protected GameManager gameManager;

    private void Start() {
        optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu").GetComponent<OptionsMenu>();
        gameManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<GameManager>();
    }

    private void Update() {
        if (gameManager.userData.currentLevel == 1 && gameManager.userData.currentCheckpointLocation == new Vector3(0, 0)) {
            continueButton.enabled = false;
            continueButton.interactable = false;
        } else {
            continueButton.enabled = true;
            continueButton.interactable = true;
        }
    }

    public void NewGame() {
        SceneManager.LoadScene(1);
        gameManager.userData.currentLevel = 1;
        gameManager.userData.currentCheckpointLocation = new Vector3(0, 0);
        gameManager.userData.score = 0;
        gameManager.SaveLevelProgress();
    }

    public void ContinueGame() {
        SceneManager.LoadScene(gameManager.userData.currentLevel);
    }

    public void GoToOptions() {
        if (optionsMenu) {
            optionsMenu.optionsMenuUI.SetActive(true);
            optionsMenu.LoadOptions();
        }
    }

    public void QuitGame() {
        Debug.Log("Maine Menu - Application.Quit();");
        Application.Quit();
    }
}
