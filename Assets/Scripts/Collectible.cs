using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Collectible : MonoBehaviour
{
    [SerializeField] public int collectibleScore = 1;
    [SerializeField] int collectibleType = 0;
    [SerializeField] bool isCollectibleTypeRandom = false;

    private int randomTypesNr = 3;
    private Animator animator;
    private GameManager gameManager;

    protected void Awake() {
        animator = GetComponent<Animator>();
    }

    protected void Start() {
        gameManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<GameManager>();
        if (gameManager.userData.currentCheckpointLocation != null && (gameManager.userData.currentCheckpointLocation.x > gameObject.transform.position.x)) {
            DestroyCollectible();
        }
        if (isCollectibleTypeRandom) {
            collectibleType = Random.Range(0, randomTypesNr + 1);
        }
        animator.SetFloat("AnimationType", collectibleType);
    }

    public void StartAnimation() {
        GetComponent<CircleCollider2D>().enabled = false;
        animator.SetTrigger("Collected");
    }

    // Called from the collected animation
    protected void DestroyCollectible() {
        Destroy(gameObject);
    }
}
