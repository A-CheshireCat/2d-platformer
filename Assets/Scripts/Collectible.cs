using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] public int collectibleScore = 1;
    [SerializeField] int collectibleType = 0;
    [SerializeField] bool isCollectibleTypeRandom = false;

    private int randomTypesNr = 3;
    private Animator animator;

    protected void Awake() {
        animator = GetComponent<Animator>();
    }

    protected void Start() {
        if (isCollectibleTypeRandom) {
            collectibleType = Random.Range(0, randomTypesNr + 1);
        }
        animator.SetFloat("AnimationType", collectibleType);
    }

    public void StartAnimation() {
        animator.SetTrigger("Collected");
    }

    // Called from the collected animation
    protected void DestroyCollectible() {
        Destroy(gameObject);
    }
}
