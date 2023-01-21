using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    // PlayerPrefs keys
    private const string MAIN_VOLUME = "mainVolume";
    private const string MUSIC_VOLUME = "musicVolume";
    private const string EFFECTS_VOLUME = "effectsVolume";

    private const string IS_FULLSCREEN = "isFullscreen";

    private const string CURRENT_LEVEL = "currentLevel";
    private const string CURRENT_CHECKPOINT_X = "currentCheckpointLocation.x";
    private const string CURRENT_CHECKPOINT_Y = "currentCheckpointLocation.y";
    private const string SCORE = "score";

    public UserData userData;
    public AudioMixer audioMixer;

    private static GameManager instance;

    private void Awake() {
        if (instance == null) {
            userData = new UserData();
            instance = this;
            
            DontDestroyOnLoad(instance);
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        Load();
        // SETUP ALL THE SAVED OPTIONS ON GAME START
        StartSetup();
    }

    private void OnApplicationQuit() {
        // If we quit between checkpoints we don't wanna keep intermediary score
        userData.score = PlayerPrefs.GetInt(SCORE, 0);

        Save();
    }

    private void StartSetup() {
        audioMixer.SetFloat("MasterVolume", userData.mainVolume);
        audioMixer.SetFloat("MusicVolume", userData.musicVolume);
        audioMixer.SetFloat("EffectsVolume", userData.effectsVolume);
        Screen.fullScreen = userData.isFullscreen;
    }

    private void Save() {
        Debug.Log("gamemanager.save all in player prefs");
        SaveOptions();
        SaveLevelProgress();
    }

    private void Load() {
        Debug.Log("gamemanager.load all in player prefs");
        LoadOptions();
        LoadLevelProgress();
    }

    public void SaveOptions() {
        Debug.Log("gamemanager.save options in player prefs");
        PlayerPrefs.SetFloat(MAIN_VOLUME, userData.mainVolume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME, userData.musicVolume);
        PlayerPrefs.SetFloat(EFFECTS_VOLUME, userData.effectsVolume);

        PlayerPrefs.SetString(IS_FULLSCREEN, userData.isFullscreen.ToString());

        PlayerPrefs.Save();
    }

    public void SaveLevelProgress() {
        Debug.Log("gamemanager.save level progress in player prefs");
        PlayerPrefs.SetInt(CURRENT_LEVEL, userData.currentLevel); 
        PlayerPrefs.SetFloat(CURRENT_CHECKPOINT_X, userData.currentCheckpointLocation.x);
        PlayerPrefs.SetFloat(CURRENT_CHECKPOINT_Y, userData.currentCheckpointLocation.y);
        PlayerPrefs.SetInt(SCORE, userData.score);

        PlayerPrefs.Save();
    }

    public void LoadOptions() {
        Debug.Log("gamemanager.load options in player prefs");
        // Check if we have what to load
        if (PlayerPrefs.HasKey(MAIN_VOLUME)) {
            userData.mainVolume = PlayerPrefs.GetFloat(MAIN_VOLUME, 0);
            userData.musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME, 0);
            userData.effectsVolume = PlayerPrefs.GetFloat(EFFECTS_VOLUME, 0);

            userData.isFullscreen = PlayerPrefs.GetString(IS_FULLSCREEN, "true") == "true";
        }
    }

    public void LoadLevelProgress() {
        Debug.Log("gamemanager.load level progress in player prefs");
        // Check if we have what to load
        if (PlayerPrefs.HasKey(CURRENT_LEVEL)) {
            userData.currentLevel = PlayerPrefs.GetInt(CURRENT_LEVEL, 1);
            userData.currentCheckpointLocation = new Vector3(PlayerPrefs.GetFloat(CURRENT_CHECKPOINT_X, 0),
                                                             PlayerPrefs.GetFloat(CURRENT_CHECKPOINT_Y, 0));
            userData.score = PlayerPrefs.GetInt(SCORE, 0);
        }
    }
}
