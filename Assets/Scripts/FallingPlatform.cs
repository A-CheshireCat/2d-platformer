using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] float fallingTime = 2.0f;

    private Animator animator;

    protected void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Collision with fall platform");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            animator.SetTrigger("IsFalling");
            Destroy(gameObject, fallingTime);
        }
    }
}
