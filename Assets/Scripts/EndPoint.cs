using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour
{
    protected GameManager gameManager;

    protected void Start() {
        gameManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<GameManager>();
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            gameManager.userData.currentLevel = SceneManager.GetActiveScene().buildIndex + 1;
            gameManager.userData.currentCheckpointLocation = new Vector3(0, 0);
            gameManager.userData.score = 0;
            gameManager.SaveLevelProgress();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
