using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSurface : StickySurface
{
    protected override void OnCollisionEnter2D(Collision2D collision) {
        base.OnCollisionEnter2D(collision);

        PopBubble(collision);
    }

    protected void OnCollisionStay2D(Collision2D collision) {
        PopBubble(collision);
    }

    private void PopBubble(Collision2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            PlayerWaterController playerController = collision.gameObject.GetComponent<PlayerWaterController>();
            if (playerController != null && !playerController.isWet) {
                collision.gameObject.transform.SetParent(null);
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
