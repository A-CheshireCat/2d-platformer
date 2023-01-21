using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] float degreesPerSecond = 100.0f;
    [SerializeField] Vector3 rotationAxis = new Vector3(0, 0, 1);

    void FixedUpdate() {
        transform.Rotate(rotationAxis, degreesPerSecond * Time.deltaTime);
    }
}
