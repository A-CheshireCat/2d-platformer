using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickySurface : MonoBehaviour
{
    [SerializeField] protected LayerMask groundLayerMask;

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            RaycastHit2D raycastHit = Physics2D.BoxCast(collision.collider.bounds.center, collision.collider.bounds.size, 0f, Vector2.down, 0.1f, groundLayerMask);

            if (raycastHit.collider != null) {
                collision.gameObject.transform.SetParent(transform);
            }
        }
    }

    protected void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
