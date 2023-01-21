using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [SerializeField] protected List<GameObject> waypoints;
    [SerializeField] protected bool cyclical = false ;
    [SerializeField] protected bool destroyAtEnd = false;
    [SerializeField] protected float speed = 2.0f;

    private int currentWaypointIndex = 0;
    private int listDirection = 1;

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.1f) {
            currentWaypointIndex += listDirection;

            if (currentWaypointIndex >= waypoints.Count) {
                // Reached the last waypoint
                if (destroyAtEnd) {
                    // Destroy the parent with the waypoints too
                    Destroy(transform.parent.gameObject);
                } else if (cyclical) {
                    // Go round in a circle through the waypoint from the start
                    currentWaypointIndex = 0;
                } else {
                    // Start Backtracking
                    listDirection *= -1;
                    currentWaypointIndex = waypoints.Count - 2;
                }
            }

            if (currentWaypointIndex < 0) {
                // Start through the list again
                listDirection *= -1;
                currentWaypointIndex = 1;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
    }
}
