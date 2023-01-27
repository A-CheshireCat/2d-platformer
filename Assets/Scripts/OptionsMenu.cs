using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsMenuUI;

    public Slider mainVolSlider;
    public Slider musicVolSlider;
    public Slider effectsVolSlider;
    public Toggle fullscreenToggle;

    public AudioMixer audioMixer;
    
    protected GameManager gameManager;

    protected void Start() {
        gameManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<GameManager>();
    }

    private void SaveOptions() {
        gameManager.SaveOptions();
    }

    public void LoadOptions() {
        gameManager.LoadOptions();

        mainVolSlider.value = gameManager.userData.mainVolume;

        musicVolSlider.value = gameManager.userData.musicVolume;

        effectsVolSlider.value = gameManager.userData.effectsVolume;

        fullscreenToggle.isOn = gameManager.userData.isFullscreen;
    }

    #region SettingsOptions
    public void Back() {
        SaveOptions();
        optionsMenuUI.SetActive(false);
    }

    public void SetMainVolume(float mainVolume) {
        audioMixer.SetFloat("MasterVolume", mainVolume);
        gameManager.userData.mainVolume = mainVolume;
    }

    public void SetMusicVolume(float musicVolume) {
        audioMixer.SetFloat("MusicVolume", musicVolume);
        gameManager.userData.musicVolume = musicVolume;
    }

    public void SetEffectsVolume(float effectsVolume) {
        audioMixer.SetFloat("EffectsVolume", effectsVolume);
        gameManager.userData.effectsVolume = effectsVolume;
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
        gameManager.userData.isFullscreen = isFullscreen;
    }
    #endregion
}
