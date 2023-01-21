using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Checkpoint : MonoBehaviour
{
    protected Animator animator;
    protected GameManager gameManager;

    private bool isActive = false;
    public bool IsActive {
        get { return isActive; }
        set {
            if (isActive != value) {
                isActive = value;
                if(isActive) {
                    animator.SetBool("isActive", true);
                }
            }
        }
    }

    protected void Awake() {
        animator = GetComponent<Animator>();
    }

    protected void Start() {
        gameManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<GameManager>();
        // Check if it was already activated
        if (transform.position.x < gameManager.userData.currentCheckpointLocation.x) {
            IsActive = true;
        }
    }

    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !IsActive) {
            IsActive = true;
            gameManager.userData.currentCheckpointLocation = transform.position;
            gameManager.SaveLevelProgress();
        }
    }
}
