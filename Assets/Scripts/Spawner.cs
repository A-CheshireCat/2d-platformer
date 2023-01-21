using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] protected GameObject item;
    [SerializeField] protected int maxNrOfItems = 1;
    [SerializeField] protected float spawnTime = 2.0f;

    private bool isSpawning;

    protected void FixedUpdate() {
        if (transform.childCount < maxNrOfItems && !isSpawning) {
            isSpawning = true;
            Invoke("SpawnItem", spawnTime);
        }
    }

    private void SpawnItem() {
        Instantiate(item, transform.position, transform.rotation, transform);
        isSpawning = false;
    }
}
