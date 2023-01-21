using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData {
    //Options
    public float mainVolume = 0;
    public float musicVolume = 0;
    public float effectsVolume = 0;

    public bool isFullscreen = true;

    //Game Data
    public int currentLevel = 1;
    public Vector3 currentCheckpointLocation = new Vector3(0,0);
    public int score = 0;
}