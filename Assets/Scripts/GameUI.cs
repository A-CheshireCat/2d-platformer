using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Text levelText;
    [SerializeField] private Text scoreText;

    private string level = "Level ";
    private string score = "Score: ";
    protected GameManager gameManager;

    protected void Start() {
        gameManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<GameManager>();
        levelText.text = level + gameManager.userData.currentLevel.ToString();
        scoreText.text = score + gameManager.userData.score.ToString();
    }

    public void UpdateScoreText(int newScore) {
        scoreText.text = score + newScore;
        Debug.Log("gameUI. score should be " + newScore);
    }
}
