using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanCurrent : MonoBehaviour
{
    protected void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            PlayerWaterController pwc = other.gameObject.GetComponent<PlayerWaterController>();
            //Only air fans should be affected
            if (pwc != null && pwc.isWet && !pwc.IsInWater) {
                gameObject.GetComponent<AreaEffector2D>().enabled = false;
            } else {
                gameObject.GetComponent<AreaEffector2D>().enabled = true;
            }
        }
    }

    protected void OnTriggerStay2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            PlayerWaterController pwc = collision.gameObject.GetComponent<PlayerWaterController>();
            //Only air fans should be affected
            if (pwc != null && pwc.isWet && !pwc.IsInWater) {
                gameObject.GetComponent<AreaEffector2D>().enabled = false;
            }
            else {
                gameObject.GetComponent<AreaEffector2D>().enabled = true;
            }
        }
    }
}
